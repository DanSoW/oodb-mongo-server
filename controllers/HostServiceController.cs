using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

/*
 * В данном файле представлен класс HostServiceController, 
 * который является наследником абстрактного класса BaseController.
 * Тип "входной" модели для данных, курсирующих внутри системы является HostServiceModel
 * Тип "выходной" модели для данных, возвращаемых пользователю является HostServiceDataModel
 * В данном классе переопределены методы Create, Update, Delete, Get и GetAll (т.е. все CRUD-операции), 
 * так как логика отдельных операций для данного класса отличается от общей схемы (определяемая абстрактным классом)
 * **/

namespace oodb_project.controllers
{
    /// <summary>
    /// Класс контроллера для таблицы HostService
    /// </summary>
    public class HostServiceController : BaseController<HostServiceModel, HostServiceDataModel>
    {
        private DbContext _db;

        public HostServiceController(DbContext db) : base(db.HostServiceList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта коллекции
        /// </summary>
        /// <param name="data">Данные коллекции</param>
        /// <returns>Обновлённый объект коллекции</returns>
        public IResult Update(HostServiceDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                // Поиск объекта в коллекции HostService
                var hostService = _collection.Find(document => document.Id == ObjectId.Parse(data.Id)).FirstOrDefault();
                if (hostService == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostService с Id = {data.Id} не обнаружен в БД"));
                }

                // Поиск объекта в коллекции Host
                var host = _db.HostList.Find(document => document.Id == ObjectId.Parse(data.HostId)).FirstOrDefault();
                if (host == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostModel с Id = {data.HostId} не обнаружен в БД"));
                }

                // Поиск объекта в коллекции Service
                var service = _db.ServiceList.Find(document => document.Id == ObjectId.Parse(data.ServiceId)).FirstOrDefault();
                if (service == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта ServiceModel с Id = {data.ServiceId} не обнаружен в БД"));
                }

                var filter = Builders<HostServiceModel>.Filter.Eq(s => s.Id, ObjectId.Parse(data.Id));
                var update = Builders<HostServiceModel>.Update
                    .Set(s => s.Host, host)
                    .Set(s => s.Service, service);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(data);
        }

        /// <summary>
        /// Создание объекта коллекции
        /// </summary>
        /// <param name="data">Данные объекта</param>
        /// <returns>Созданный объект</returns>
        public new IResult Create(HostServiceDataModel data)
        {
            if ((_collection == null) || (_db == null))
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var host = _db.HostList.Find(document => document.Id == ObjectId.Parse(data.HostId)).FirstOrDefault();
                if (host == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostModel с Id = {data.HostId} не обнаружен в БД"));
                }

                var service = _db.ServiceList.Find(document => document.Id == ObjectId.Parse(data.ServiceId)).FirstOrDefault();
                if (service == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта ServiceModel с Id = {data.ServiceId} не обнаружен в БД"));
                }

                var entity = new HostServiceModel(host, service);
                data.Id = entity.Id.ToString();

                _collection.InsertOne(entity);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(data);
        }

        /// <summary>
        /// Получение конкретного объекта коллекции
        /// </summary>
        /// <param name="id">Идентификатор объекта в коллекции</param>
        /// <returns>Объект коллекции</returns>
        public new IResult Get(string id)
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
                    return Results.Json(new MessageModel($"Экземпляра объекта {typeof(HostServiceModel).Name} с Id = {id} не обнаружен в БД"));
                }

                return Results.Json(
                    new HostServiceDataModel(data.Id.ToString(), 
                    data.Host!.Id.ToString(), 
                    data.Service!.Id.ToString())
                );
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }

        /// <summary>
        /// Получение списка объектов в коллекции
        /// </summary>
        /// <returns>Список объектов в коллекции</returns>
        public new IResult GetAll()
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var list = _collection.Find(Builders<HostServiceModel>.Filter.Empty).ToList();
                var result = new List<HostServiceDataModel>();

                foreach (var item in list)
                {
                    result.Add(new HostServiceDataModel(item.Id.ToString(), item.Host!.Id.ToString(), item.Service!.Id.ToString()));
                }

                return Results.Json(result.ToArray());
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }
    }
}
