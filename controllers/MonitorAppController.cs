using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

namespace oodb_project.controllers
{
    public class MonitorAppController : BaseController<MonitorAppModel, MonitorAppDataModel>
    {
        private DbContext _db;

        public MonitorAppController(DbContext db) : base(db.MonitorAppList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта MonitorAppModel
        /// </summary>
        public IResult Update(MonitorAppDataModel newData)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var monitorApp = _db.MonitorAppList.Find(document => document.Id == ObjectId.Parse(newData.Id)).FirstOrDefault();
                if (monitorApp == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта MonitorAppModel с Id = {newData.Id} не обнаружен в БД"));
                }

                var host = _db.HostList.Find(document => document.Id == ObjectId.Parse(newData.HostId)).FirstOrDefault();
                if (host == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostModel с Id = {newData.HostId} не обнаружен в БД"));
                }

                var admin = _db.AdminList.Find(document => document.Id == ObjectId.Parse(newData.AdminId)).FirstOrDefault();
                if (admin == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта AdminModel с Id = {newData.AdminId} не обнаружен в БД"));
                }

                var filter = Builders<MonitorAppModel>.Filter.Eq(s => s.Id, ObjectId.Parse(newData.Id));
                var update = Builders<MonitorAppModel>.Update
                    .Set(s => s.Name, newData.Name)
                    .Set(s => s.Url, newData.Url)
                    .Set(s => s.Host, host)
                    .Set(s => s.Admin, admin);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(newData);
        }

        /// <summary>
        /// Создание объекта MonitorAppModel
        /// </summary>
        public IResult Create(MonitorAppDataModel data)
        {
            if (_collection == null)
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

                var admin = _db.AdminList.Find(document => document.Id == ObjectId.Parse(data.AdminId)).FirstOrDefault();
                if (admin == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта AdminModel с Id = {data.AdminId} не обнаружен в БД"));
                }

                var entity = new MonitorAppModel(data.Name, data.Url, host, admin);
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
                    return Results.Json(new MessageModel($"Экземпляра объекта {typeof(MonitorAppModel).Name} с Id = {id} не обнаружен в БД"));
                }

                return Results.Json(
                    new MonitorAppDataModel(data.Id.ToString(), data.Name, data.Url, data.Host!.Id.ToString(), data.Admin!.Id.ToString())
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
                var list = _collection.Find(Builders<MonitorAppModel>.Filter.Empty).ToList();
                var result = new List<MonitorAppDataModel>();

                foreach (var item in list)
                {
                    result.Add(new MonitorAppDataModel(item.Id.ToString(), item.Name, item.Url,
                        item.Host!.Id.ToString(), item.Admin!.Id.ToString()));
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
