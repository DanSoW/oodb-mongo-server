using MongoDB.Bson;
using MongoDB.Driver;
using oodb_mongo_server.controllers;
using oodb_mongo_server.database.context;
using oodb_project.models;

/*
 * В данном файле представлен класс DataSource, 
 * который является наследником абстрактного класса BaseController.
 * Тип "входной" модели для данных, курсирующих внутри системы является DataSourceModel
 * Тип "выходной" модели для данных, возвращаемых пользователю является DataSourceDataModel
 * В данном классе переопределены методы Update и Delete, т.к. логика данных методов
 * может различаться от класса к классу и не подлежит обобщению в рамках текущей системы.
 * Также в данном классе переопределён метод Create, т.к. требуется явное указание параметров
 * метода при отправки его контроллеру.
 * Методы Get, GetAll не переопределены и используются из абстрактного класса.
 * **/

namespace oodb_project.controllers
{
    /// <summary>
    /// Класс контроллера для таблицы DataSource
    /// </summary>
    public class DataSourceController : BaseController<DataSourceModel, DataSourceDataModel>
    {
        private DbContext _db;

        public DataSourceController(DbContext db) : base(db.DataSourceList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта коллекции
        /// </summary>
        /// <param name="data">Новые данные для объекта в коллекции</param>
        /// <returns>Обновлённый объект коллекции</returns>
        public IResult Update(DataSourceDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var dataSource = _collection.Find(document => document.Id == ObjectId.Parse(data.Id)).FirstOrDefault();
                if (dataSource == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта DataSource с Id = {data.Id} не обнаружен в БД"));
                }

                var filter = Builders<DataSourceModel>.Filter.Eq(s => s.Id, ObjectId.Parse(data.Id));
                var update = Builders<DataSourceModel>.Update
                    .Set(s => s.Url, data.Url)
                    .Set(s => s.Name, data.Name);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(data);
        }

        /// <summary>
        /// Создание объекта в коллекции
        /// </summary>
        /// <param name="data">Данные объекта</param>
        /// <returns>Созданный объект</returns>
        public new IResult Create(DataSourceDataModel data)
        {
            return base.Create(data);
        }

        /// <summary>
        /// Удаление объекта коллекции
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Удалённый объект</returns>
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
