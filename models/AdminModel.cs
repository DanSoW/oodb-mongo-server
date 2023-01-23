using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using oodb_mongo_server.models;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая администратора приложения для мониторинга
    /// </summary>
    public class AdminModel : IdModel
    {
        public AdminModel() : base() {}

        public AdminModel(ObjectId? id, string? email) : base(id)
        {
            Email = email;
        }

        public AdminModel(string? email) : base()
        {
            Email = email;
        }

        public string? Email { get; set; }
    }
}
