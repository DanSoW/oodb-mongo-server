using oodb_mongo_server.models.data;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая конкретный удалённый хост
    /// </summary>
    public class HostDataModel : IdDataModel
    {
        public HostDataModel() : base()
        {
        }

        public HostDataModel(string? id, string? name, string? url, string? iPv4, string? system) : base(id)
        {
            Name = name;
            Url = url;
            IPv4 = iPv4;
            System = system;
        }

        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? IPv4 { get; set; }
        public string? System { get; set; }

    }
}
