using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using oodb_mongo_server.models;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая приложения для мониторинга состояния удалённых хостов
    /// </summary>
    public class MonitorAppModel : IdModel
    {
        public MonitorAppModel() : base()
        {
        }

        public MonitorAppModel(ObjectId? id, string? name, string? url, HostModel? host, AdminModel? admin) : base()
        {
            Id = id;
            Name = name;
            Url = url;
            Host = host;
            Admin = admin;
        }

        public MonitorAppModel(string? name, string? url, HostModel? host, AdminModel? admin) : base()
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Url = url;
            Host = host;
            Admin = admin;
        }

        public string? Name { get; set; }
        public string? Url { get; set; }
        public HostModel? Host { get; set; }
        public AdminModel? Admin { get; set; }
    }
}
