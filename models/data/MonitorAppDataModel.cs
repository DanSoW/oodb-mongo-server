using oodb_mongo_server.models.data;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая приложения для мониторинга состояния удалённых хостов
    /// </summary>
    public class MonitorAppDataModel : IdDataModel
    {
        public MonitorAppDataModel() : base()
        {
        }

        public MonitorAppDataModel(string? id, string? name, string? url, string? hostId, string? adminId) : base(id)
        {
            Name = name;
            Url = url;
            HostId = hostId;
            AdminId = adminId;
        }

        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? HostId { get; set; }
        public string? AdminId { get; set; }
    }
}
