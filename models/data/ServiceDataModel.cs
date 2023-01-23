using oodb_mongo_server.models.data;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая отдельный сервис (микросервис)
    /// </summary>
    public class ServiceDataModel : IdDataModel
    {
        public ServiceDataModel() : base()
        {
        }

        public ServiceDataModel(string? id, string? name, int port, int timeUpdate, string? dataSourceId) : base(id)
        {
            Name = name;
            Port = port;
            TimeUpdate = timeUpdate;
            DataSourceId = dataSourceId;
        }

        public string? Name { get; set; }
        public int Port { get; set; }

        public int TimeUpdate { get; set; }

        public string? DataSourceId { get; set; }
    }
}
