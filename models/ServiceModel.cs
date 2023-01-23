using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using oodb_mongo_server.models;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая отдельный сервис (микросервис)
    /// </summary>
    public class ServiceModel : IdModel
    {
        public ServiceModel() : base()
        {
        }

        public ServiceModel(ObjectId? id, string? name, int port, int timeUpdate, DataSourceModel? dataSource) : base(id)
        {
            Name = name;
            Port = port;
            TimeUpdate = timeUpdate;
            DataSource = dataSource;
        }

        public ServiceModel(string? name, int port, int timeUpdate, DataSourceModel? dataSource) : base()
        {
            Name = name;
            Port = port;
            TimeUpdate = timeUpdate;
            DataSource = dataSource;
        }

        public string? Name { get; set; }
        public int Port { get; set; }
        public int TimeUpdate { get; set; }
        public DataSourceModel? DataSource { get; set; }
    }
}
