using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

namespace oodb_project.controllers
{
    public class ServiceController : BaseController<ServiceModel, ServiceDataModel>
    {
        private DbContext _db;

        public ServiceController(DbContext db) : base(db.ServiceList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта ServiceModel
        /// </summary>
        public IResult Update(ServiceDataModel newData)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var service = _db.ServiceList.Find(document => document.Id == ObjectId.Parse(newData.Id)).FirstOrDefault();

                if (service == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта ServiceModel с Id = {newData.Id} не обнаружен в БД"));
                }

                var dataSource = _db.DataSourceList.Find(document => document.Id == ObjectId.Parse(newData.DataSourceId)).FirstOrDefault();

                if (dataSource == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта DataSourceModel с Id = {newData.DataSourceId} не обнаружен в БД"));
                }

                var filter = Builders<ServiceModel>.Filter.Eq(s => s.Id, ObjectId.Parse(newData.Id));
                var update = Builders<ServiceModel>.Update
                    .Set(s => s.Name, newData.Name)
                    .Set(s => s.Port, newData.Port)
                    .Set(s => s.TimeUpdate, newData.TimeUpdate)
                    .Set(s => s.DataSource, dataSource);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(newData);
        }

        /// <summary>
        /// Создание объекта ServiceModel
        /// </summary>
        public IResult Create(ServiceDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var dataSource = _db.DataSourceList.Find(document => document.Id == ObjectId.Parse(data.DataSourceId)).FirstOrDefault();

                if (dataSource == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта DataSourceModel с Id = {data.DataSourceId} не обнаружен в БД"));
                }

                var entity = new ServiceModel(data.Name, data.Port, data.TimeUpdate, dataSource);
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
                    return Results.Json(new MessageModel($"Экземпляра объекта {typeof(ServiceDataModel).Name} с Id = {id} не обнаружен в БД"));
                }

                return Results.Json(
                    new ServiceDataModel(data.Id.ToString(), data.Name, data.Port, data.TimeUpdate, data.DataSource!.Id.ToString())
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
                var list = _collection.Find(Builders<ServiceModel>.Filter.Empty).ToList();
                var result = new List<ServiceDataModel>();

                foreach (var item in list)
                {
                    result.Add(new ServiceDataModel(item.Id.ToString(), item.Name, item.Port, item.TimeUpdate, item.DataSource!.Id.ToString()));
                }

                return Results.Json(result.ToArray());
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }

        /// <summary>
        /// Метод для удаления модели
        /// </summary>
        /// <param name="id">Идентификатор модели</param>
        /// <returns>Удалённая модель (данные, которые были до удаления в модели)</returns>
        public new IResult Delete(string id)
        {
            if ((_collection == null) || (_db.HostServiceList == null))
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
                _db.HostServiceList.DeleteMany(x => x.Service!.Id == data.Id);

                return Results.Json(
                    new ServiceDataModel(data.Id.ToString()!, data.Name!, data.Port!, 
                    data.TimeUpdate, data.DataSource!.Id.ToString()
                    )
                );
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }
    }
}
