using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using oodb_mongo_server.models;

namespace oodb_project.models
{
    /// <summary>
    /// Модель связки конкретных сервисов с конкретными хостами
    /// </summary>
    public class HostServiceModel : IdModel
    {
        public HostServiceModel() : base()
        {
        }

        public HostServiceModel(ObjectId? id, HostModel? host, ServiceModel? service) : base(id)
        {
            Host = host;
            Service = service;
        }

        public HostServiceModel(HostModel? host, ServiceModel? service) : base()
        {
            Host = host;
            Service = service;
        }

        public HostModel? Host { get; set; }
        public ServiceModel? Service { get; set; }
    }
}
