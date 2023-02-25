using oodb_mongo_server.models;
using MongoDB.Driver;
using oodb_mongo_server.models.data;
using oodb_project.models;
using oodb_mongo_server.utils;
using MongoDB.Bson;
using System.Reflection;


/*
 * В данном файле определён абстрактный класс BaseController, который
 * использует механизм обобщений.
 * Для данного класса необходимо указать тип для входных параметров и выходных параметров
 * Абстрактный класс определяет методы Get, GetAll и Create
 * **/

namespace oodb_mongo_server.controllers
{
    /// <summary>
    /// Абстрактный класс для контроллеров
    /// </summary>
    public abstract class BaseController<IT, OT> 
        where IT : IdModel                          // Тип для входных параметров
        where OT : IdDataModel, new()               // Тип для выходных параметров
    {
        // Коллекция входных параметров
        protected IMongoCollection<IT>? _collection;

        /// <summary>
        /// Конструктор абстрактного класса
        /// </summary>
        /// <param name="collection">Ссылка на коллекцию</param>
        public BaseController(IMongoCollection<IT>? collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Метод для создания нового объекта коллекции
        /// </summary>
        /// <param name="data">Данные объекта коллекции</param>
        /// <returns>Созданный объект коллекции</returns>
        protected IResult Create(OT data)
        {
            // Проверка подключения к базе данных
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                // Получение всех полей выходного объекта
                var fields = ReflectionUtil.getDataFields(data);

                // Получение информации о всех полях
                var fieldsInfo = ReflectionUtil.getDataFieldsInfo(data);

                if(fields.Count > fieldsInfo.Count)
                {
                    // Удаляем id в модели
                    fields.RemoveAt(0);
                }

                // Создаём объект входного типа
                var value = Activator.CreateInstance(typeof(IT), fields.ToArray());
                if (value != null)
                {

                    // Добавление объекта в коллекцию
                    _collection.InsertOne((IT)value);

                    var fieldsValue = ReflectionUtil.getFields((IT)value);

                    // Процедура замены id входной модели (ObjectId) на id выходной модели (String)
                    var id = fieldsValue[0].ToString();
                    fieldsValue.RemoveAt(0);
                    if (id != null)
                    {
                        fieldsValue.Insert(0, id);
                    }

                    var result = Activator.CreateInstance(typeof(OT), fieldsValue.ToArray());

                    // Возвращаем результат поиска, если при создании объекта не получилось значение null
                    return Results.Json(result);
                }

                // Возвращаем ошибку
                return Results.Json(new MessageModel("Ошибка: невозможно создать объект для коллекции"));
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }

        private bool methodName(FieldInfo obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод для получения всех объектов определённого типа
        /// </summary>
        /// <returns>Результат работы функции (массив документов)</returns>
        public IResult GetAll()
        {
            // Проверка подключения к базе данных
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                // Поиск всех элементов в коллекции определённого типа (входной тип)
                var list = _collection.Find(Builders<IT>.Filter.Empty).ToList();

                // Результат представлен в отдельной структуре, т.к. данные нужно преобразовать перед отправкой пользователю
                var result = new List<OT>();

                foreach (var item in list)
                {
                    // Получение всех полей элемента входного типа (IT)
                    var fields = ReflectionUtil.getFields(item);

                    // Процедура замены id входной модели (ObjectId) на id выходной модели (String)
                    var id = fields[0].ToString();
                    fields.RemoveAt(0);
                    if(id != null)
                    {
                        fields.Insert(0, id);
                    }

                    // Создание нового объекта выходного типа OT с передачей в его конструктор определённых полей
                    var value = Activator.CreateInstance(typeof(OT), fields.ToArray());

                    // Если value не равен null, то добавляем его в список результатов
                    if(value != null)
                    {
                        result.Add((OT)value);
                    }
                }

                // Возвращаем преобразованный массив результатов
                return Results.Json(result.ToArray());
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }

        /// <summary>
        /// Метод для получения конкретного объекта из коллекции
        /// </summary>
        /// <param name="id">Идентификатор искомого объекта</param>
        /// <returns>Результат работы метода (конкретный объект)</returns>
        public IResult Get(string id)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                // Поиск конкретного документа по его идентификатору в коллекции
                var data = _collection.Find(document => document.Id == ObjectId.Parse(id)).FirstOrDefault();

                // Проверка обнаружения объекта в коллекции (при отсутствии возвращаем ошибку)
                if (data == null)
                {
                    return Results.Json(new MessageModel($"Экземпляр объекта {typeof(IT).Name} с Id = {id} не обнаружен в БД"));
                }

                // Преобразование объекта входного типа в объект выходного типа
                var fields = ReflectionUtil.getFields(data);
                fields.RemoveAt(0);
                if (id != null)
                {
                    fields.Insert(0, id);
                }

                // Создаём объект выходного типа
                var value = Activator.CreateInstance(typeof(OT), fields.ToArray());
                if (value != null)
                {
                    // Возвращаем результат поиска, если при создании объекта не получилось значение null
                    return Results.Json((OT)value);
                }

                return Results.Json("Internal Server Error");
            }
            catch (Exception)
            {
                return Results.Json(new MessageModel($"Экземпляра объекта {typeof(IT).Name} с Id = {id} не обнаружен в БД"));
            }
        }

        /// <summary>
        /// Метод для удаления объекта из коллекции
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Удалённый объект коллекции</returns>
        public IResult Delete(string id)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var data = _collection.Find(document => document.Id == ObjectId.Parse(id)).FirstOrDefault();
                if (data == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта {typeof(IT).Name} с Id = {id} не обнаружен в БД"));
                }

                // Удаление объекта из коллекции
                _collection.DeleteOne(x => x.Id == data.Id);

                var fields = ReflectionUtil.getFields(data);
                fields.RemoveAt(0);
                if (id != null)
                {
                    fields.Insert(0, id);
                }

                var value = Activator.CreateInstance(typeof(OT), fields.ToArray());
                if (value != null)
                {
                    return Results.Json((OT)value);
                }

                return Results.Json("Internal Server Error");
            }
            catch (Exception)
            {
                return Results.Json(new MessageModel($"Экземпляра объекта {typeof(IT).Name} с Id = {id} не обнаружен в БД"));
            }
        }
    }
}
