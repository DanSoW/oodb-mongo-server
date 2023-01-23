using oodb_mongo_server.models.data;

namespace oodb_project.models
{
    /// <summary>
    /// Модель связки конкретных сервисов с конкретными хостами
    /// </summary>
    public class HostServiceDataModel : IdDataModel
    {
        public HostServiceDataModel() : base()
        {
        }

        public HostServiceDataModel(string? id, string? hostId, string? serviceId) : base(id)
        {
            HostId = hostId;
            ServiceId = serviceId;
        }

        public string? HostId { get; set; }
        public string? ServiceId { get; set; }
    }
}
