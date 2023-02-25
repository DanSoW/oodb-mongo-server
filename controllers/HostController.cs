using oodb_mongo_server.database.context;
using oodb_project.models;
using MongoDB.Driver;
using MongoDB.Bson;
using oodb_mongo_server.controllers;

/*
 * В данном файле представлен класс HostController, 
 * который является наследником абстрактного класса BaseController.
 * Тип "входной" модели для данных, курсирующих внутри системы является HostModel
 * Тип "выходной" модели для данных, возвращаемых пользователю является HostDataModel
 * В данном классе переопределены методы Update и Delete, т.к. логика данных методов
 * может различаться от класса к классу и не подлежит обобщению в рамках текущей системы.
 * Также в данном классе переопределён метод Create, т.к. требуется явное указание параметров
 * метода при отправки его контроллеру.
 * Методы Get, GetAll не переопределены и используются из абстрактного класса.
 * **/

namespace oodb_project.controllers
{
    /// <summary>
    /// Класс контроллера для таблицы HostController
    /// </summary>
    public class HostController : BaseController<HostModel, HostDataModel>
    {
        private DbContext _db;

        public HostController(DbContext db) : base(db.HostList)
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта в коллекции
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IResult Update(HostDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var findObj = _collection.Find(document => document.Id == ObjectId.Parse(data.Id)).FirstOrDefault();
                if (findObj == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта HostModel с Id = {data.Id} не обнаружен в БД"));
                }

                var filter = Builders<HostModel>.Filter.Eq(s => s.Id, ObjectId.Parse(data.Id));
                var update = Builders<HostModel>.Update
                    .Set(s => s.Name, data.Name)
                    .Set(s => s.Url, data.Url)
                    .Set(s => s.IPv4, data.IPv4)
                    .Set(s => s.System, data.System);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(data);
        }

        /// <summary>
        /// Создание нового объекта коллекции
        /// </summary>
        /// <param name="data">Данные объекта</param>
        /// <returns>Созданный объект</returns>
        public new IResult Create(HostDataModel data)
        {
            return base.Create(data);
        }

        /// <summary>
        /// Удаление объекта коллекции
        /// </summary>
        /// <param name="id">Идентификатор объекта в коллекции</param>
        /// <returns>Удалённый объект коллекции</returns>
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
