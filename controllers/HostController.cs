using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

namespace oodb_project.controllers
{
    public class HostController : BaseController<HostModel, HostDataModel>
    {
        private DbContext _db;

        public HostController(DbContext db) : base(db.HostList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта Host
        /// </summary>
        public IResult Update(HostDataModel newData)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var data = _collection.Find(document => document.Id == ObjectId.Parse(newData.Id)).FirstOrDefault();
                if (data == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostModel с Id = {newData.Id} не обнаружен в БД"));
                }

                var filter = Builders<HostModel>.Filter.Eq(s => s.Id, ObjectId.Parse(newData.Id));
                var update = Builders<HostModel>.Update
                    .Set(s => s.Name, newData.Name)
                    .Set(s => s.Url, newData.Url)
                    .Set(s => s.IPv4, newData.IPv4)
                    .Set(s => s.System, newData.System);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(newData);
        }

        /// <summary>
        /// Создание объекта Host
        /// </summary>
        public IResult Create(HostDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var entity = new HostModel(data.Name, data.Url, data.IPv4, data.System);
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
        /// Метод для удаления модели
        /// </summary>
        /// <param name="id">Идентификатор модели</param>
        /// <returns>Удалённая модель (данные, которые были до удаления в модели)</returns>
        public new IResult Delete(string id)
        {
            if ((_collection == null) 
                || (_db.HostServiceList == null)
                || (_db.MonitorAppList == null))
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var data = _collection.Find(document => document.Id == ObjectId.Parse(id)).FirstOrDefault();
                if (data == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта {typeof(DataSourceModel).Name} с Id = {id} не обнаружен в БД"));
                }

                _collection.DeleteOne(x => x.Id == data.Id);

                // Каскадное удаление
                _db.HostServiceList.DeleteMany(x => x.Host!.Id == data.Id);
                _db.MonitorAppList.DeleteMany(x => x.Host!.Id == data.Id);

                return Results.Json(new DataSourceDataModel(data.Id.ToString()!, data.Name!, data.Url!));
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }
    }
}
