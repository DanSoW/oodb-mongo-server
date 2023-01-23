using MongoDB.Bson;
using MongoDB.Driver;
using oodb_mongo_server.controllers;
using oodb_mongo_server.database.context;
using oodb_project.models;

namespace oodb_project.controllers
{
    public class DataSourceController : BaseController<DataSourceModel, DataSourceDataModel>
    {
        private DbContext _db;

        public DataSourceController(DbContext db) : base(db.DataSourceList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта DataSourceModel
        /// </summary>
        public IResult Update(DataSourceDataModel newData)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var dataSource = _collection.Find(document => document.Id == ObjectId.Parse(newData.Id)).FirstOrDefault();
                if (dataSource == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта DataSource с Id = {newData.Id} не обнаружен в БД"));
                }

                var filter = Builders<DataSourceModel>.Filter.Eq(s => s.Id, ObjectId.Parse(newData.Id));
                var update = Builders<DataSourceModel>.Update
                    .Set(s => s.Url, newData.Url)
                    .Set(s => s.Name, newData.Name);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(newData);
        }

        /// <summary>
        /// Создание объекта DataSourceModel
        /// </summary>
        public IResult Create(DataSourceDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var entity = new DataSourceModel(data.Name, data.Url);
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
                || (_db.ServiceList == null)
                || (_db.HostServiceList == null))
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
                var service = _db.ServiceList.Find(document => document.DataSource!.Id == ObjectId.Parse(id)).ToList();
                if (service != null)
                {
                    foreach(var item in service)
                    {
                        _db.ServiceList.DeleteOne(x => x.Id == item.Id);
                        _db.HostServiceList.DeleteMany(x => x.Service.Id == item.Id);
                    }
                }

                return Results.Json(new DataSourceDataModel(data.Id.ToString()!, data.Name!, data.Url!));
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }
    }
}
