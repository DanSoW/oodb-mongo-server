using MongoDB.Bson;
using MongoDB.Driver;
using oodb_mongo_server.controllers;
using oodb_mongo_server.database.context;
using oodb_project.models;

/*
 * В данном файле представлен класс AdminController, 
 * который является наследником абстрактного класса BaseController.
 * Тип "входной" модели для данных, курсирующих внутри системы является AdminModel
 * Тип "выходной" модели для данных, возвращаемых пользователю является AdminDataModel
 * В данном классе переопределены методы Update и Delete, т.к. логика данных методов
 * может различаться от класса к классу и не подлежит обобщению в рамках текущей системы.
 * Также в данном классе переопределён метод Create, т.к. требуется явное указание параметров
 * метода при отправки его контроллеру.
 * Методы Get, GetAll не переопределены и используются из абстрактного класса.
 * **/

namespace oodb_project.controllers
{
    /// <summary>
    /// Класс контроллера для таблицы Admin
    /// </summary>
    public class AdminController : BaseController<AdminModel, AdminDataModel>
    {
        // Контекст базы данных
        private DbContext _db;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        public AdminController(DbContext db) : base(db.AdminList) // Вызываем конструктор абстрактного класса
        {
            _db = db;
        }

        /// <summary>
        /// Обновление объекта в коллекции Admin
        /// </summary>
        /// <param name="data">Новые данные для объекта</param>
        /// <returns>Данные обновлённого объекта</returns>
        public IResult Update(AdminDataModel data)
        {
            if (_collection == null)
            {
                return Results.Json(new MessageModel("Подключение к ООБД отсутствует"));
            }

            try
            {
                // Поиск конкретного объекта
                var admin = _collection.Find(document => document.Id == ObjectId.Parse(data.Id)).FirstOrDefault();
                if (admin == null)
                {
                    return Results.Json(new MessageModel($"Экземпляра объекта AdminModel с Id = {data.Id} не обнаружен в БД"));
                }

                // Создание фильтра для поиска объекта в коллекции
                var filter = Builders<AdminModel>.Filter.Eq(s => s.Id, ObjectId.Parse(data.Id));

                // Создаём определение для обновления объекта в коллекции
                var update = Builders<AdminModel>.Update.Set(s => s.Email, data.Email);

                // Обновляем объект в коллекции по определённому фильтру
                _collection.UpdateOne(filter, update);
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }

            return Results.Json(data);
        }

        /// <summary>
        /// Создание объекта для коллекции Admin
        /// </summary>
        /// <param name="data">Данные объекта</param>
        /// <returns>Созданный объект</returns>
        public new IResult Create(AdminDataModel data)
        {
            return base.Create(data);
        }

        /// <summary>
        /// Удаление объекта из коллекции
        /// </summary>
        /// <param name="id">Идентификатор объекта в коллекции</param>
        /// <returns>Удалённый объект</returns>
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

                // Удаление элемента в коллекции
                _collection.DeleteOne(x => x.Id == data.Id);

                // Каскадное удаление (удаляет все записи в коллекции MonitorApp, которые связаны с текущим объектом Admin)
                _db.MonitorAppList.DeleteMany(x => x.Admin!.Id == data.Id);

                // Возвращение удалённой модели
                return Results.Json(new AdminDataModel(data.Id.ToString()!, data.Email!));
            }
            catch (Exception e)
            {
                return Results.Json(new MessageModel(e.Message));
            }
        }
    }
}
