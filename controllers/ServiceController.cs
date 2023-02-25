using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

/*
 * В данном файле представлен класс ServiceController, 
 * который является наследником абстрактного класса BaseController.
 * Тип "входной" модели для данных, курсирующих внутри системы является ServiceModel
 * Тип "выходной" модели для данных, возвращаемых пользователю является ServiceDataModel
 * В данном классе переопределены методы Create, Update, Delete, Get и GetAll (т.е. все CRUD-операции), 
 * так как логика отдельных операций для данного класса отличается от общей схемы (определяемая абстрактным классом)
 * **/

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
        /// Обновление объекта коллекции
        /// </summary>
        /// <param name="data">Данные об объекте</param>
        /// <returns>Обновлённый объект</returns>
        public IResult Update(ServiceDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var service = _db.ServiceList.Find(document => document.Id == ObjectId.Parse(data.Id)).FirstOrDefault();

                if (service == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта ServiceModel с Id = {data.Id} не обнаружен в БД"));
                }

                var dataSource = _db.DataSourceList.Find(document => document.Id == ObjectId.Parse(data.DataSourceId)).FirstOrDefault();

                if (dataSource == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта DataSourceModel с Id = {data.DataSourceId} не обнаружен в БД"));
                }

                var filter = Builders<ServiceModel>.Filter.Eq(s => s.Id, ObjectId.Parse(data.Id));
                var update = Builders<ServiceModel>.Update
                    .Set(s => s.Name, data.Name)
                    .Set(s => s.Port, data.Port)
                    .Set(s => s.TimeUpdate, data.TimeUpdate)
                    .Set(s => s.DataSource, dataSource);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(data);
        }

        /// <summary>
        /// Создание нового объекта
        /// </summary>
        /// <param name="data">Данные объекта</param>
        /// <returns>Созданный объект</returns>
        public new IResult Create(ServiceDataModel data)
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
        /// Получение объекта коллекции
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
        /// Получение списка объектов коллекции
        /// </summary>
        /// <returns>Список объектов коллекции</returns>
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
        /// Удаление объекта в коллекции
        /// </summary>
        /// <param name="id">Идентификатор объекта в коллекции</param>
        /// <returns>Удалённый объект коллекции</returns>
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
