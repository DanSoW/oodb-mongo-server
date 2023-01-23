using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

namespace oodb_project.controllers
{
    public class HostServiceController : BaseController<HostServiceModel, HostServiceDataModel>
    {
        private DbContext _db;

        public HostServiceController(DbContext db) : base(db.HostServiceList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта HostServiceModel
        /// </summary>
        public IResult Update(HostServiceDataModel newData)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var hostService = _collection.Find(document => document.Id == ObjectId.Parse(newData.Id)).FirstOrDefault();
                if (hostService == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostService с Id = {newData.Id} не обнаружен в БД"));
                }

                var host = _db.HostList.Find(document => document.Id == ObjectId.Parse(newData.HostId)).FirstOrDefault();
                if (host == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostModel с Id = {newData.HostId} не обнаружен в БД"));
                }

                var service = _db.ServiceList.Find(document => document.Id == ObjectId.Parse(newData.ServiceId)).FirstOrDefault();
                if (service == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта ServiceModel с Id = {newData.ServiceId} не обнаружен в БД"));
                }

                var filter = Builders<HostServiceModel>.Filter.Eq(s => s.Id, ObjectId.Parse(newData.Id));
                var update = Builders<HostServiceModel>.Update
                    .Set(s => s.Host, host)
                    .Set(s => s.Service, service);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(newData);
        }

        /// <summary>
        /// Создание объекта HostServiceModel
        /// </summary>
        public IResult Create(HostServiceDataModel data)
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
        /// Получение объекта
        /// </summary>
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
        /// Получение всех объектов MonitorAppModel
        /// </summary>
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
