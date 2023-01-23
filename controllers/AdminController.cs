using MongoDB.Bson;
using MongoDB.Driver;
using oodb_mongo_server.controllers;
using oodb_mongo_server.database.context;
using oodb_project.models;

namespace oodb_project.controllers
{
    public class AdminController : BaseController<AdminModel, AdminDataModel>
    {
        private DbContext _db;

        public AdminController(DbContext db) : base(db.AdminList) 
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта AdminModel
        /// </summary>
        public IResult Update(AdminDataModel newData)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var admin = _collection.Find(document => document.Id == ObjectId.Parse(newData.Id)).FirstOrDefault();
                if (admin == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта AdminModel с Id = {newData.Id} не обнаружен в БД"));
                }

                var filter = Builders<AdminModel>.Filter.Eq(s => s.Id, ObjectId.Parse(newData.Id));
                var update = Builders<AdminModel>.Update.Set(s => s.Email, newData.Email);

                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(newData);
        }

        /// <summary>
        /// Создание объекта AdminModel
        /// </summary>
        public IResult Create(AdminDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var entity = new AdminModel(data.Email);
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
            if ((_collection == null) || (_db.MonitorAppList == null))
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                var data = _collection.Find(document => document.Id == ObjectId.Parse(id)).FirstOrDefault();
                if (data == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта {typeof(AdminModel).Name} с Id = {id} не обнаружен в БД"));
                }

                _collection.DeleteOne(x => x.Id == data.Id);

                // Каскадное удаление
                _db.MonitorAppList.DeleteMany(x => x.Admin!.Id == data.Id);

                return Results.Json(new AdminDataModel(data.Id.ToString()!, data.Email!));
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }
    }
}
