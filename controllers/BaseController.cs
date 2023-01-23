using oodb_mongo_server.models;
using MongoDB.Driver;
using oodb_mongo_server.models.data;
using oodb_project.models;
using oodb_mongo_server.utils;
using MongoDB.Bson;

namespace oodb_mongo_server.controllers
{
    /// <summary>
    /// Абстрактный класс для контроллеров API
    /// </summary>
    public abstract class BaseController<IT, OT> 
        where IT : IdModel
        where OT : IdDataModel, new()
    {
        protected IMongoCollection<IT>? _collection;

        public BaseController(IMongoCollection<IT>? collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Метод для получения всех документов определённого типа
        /// </summary>
        /// <returns>Результат работы функции (массив документов)</returns>
        public IResult GetAll()
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var list = _collection.Find(Builders<IT>.Filter.Empty).ToList();
                var result = new List<OT>();

                foreach (var item in list)
                {
                    // Получение всех полей модели
                    var fields = ReflectionUtil.getFields(item);
                    var id = fields[0].ToString(); // Конвертация первого атрибута модели (id) в тип string
                    fields.RemoveAt(0);

                    if(id != null)
                    {
                        fields.Insert(0, id);
                    }

                    // Создание нового объекта типа OT с передачей в его конструктор определённых полей
                    var value = Activator.CreateInstance(typeof(OT), fields.ToArray());

                    if(value != null)
                    {
                        result.Add((OT)value);
                    }
                }

                return Results.Json(result.ToArray());
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }

        /// <summary>
        /// Метод для получения конкретного экземпляра модели
        /// </summary>
        /// <param name="id">Идентификатор искомой модели</param>
        /// <returns>Результат работы метода (конкретная модель)</returns>
        public IResult Get(string id)
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

        /// <summary>
        /// Удаление конкретной модели
        /// </summary>
        /// <param name="id">Идентификатор модели, которую необходимо удалить</param>
        /// <returns>Результат удаления (данные модели до удаления)</returns>
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
