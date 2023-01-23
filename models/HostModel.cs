using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using oodb_mongo_server.models;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая конкретный удалённый хост
    /// </summary>
    public class HostModel : IdModel
    {
        public HostModel() : base()
        {
        }

        public HostModel(ObjectId? id, string? name, string? url, string? iPv4, string? system) : base(id)
        {
            Name = name;
            Url = url;
            IPv4 = iPv4;
            System = system;
        }

        public HostModel(string? name, string? url, string? iPv4, string? system) : base()
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
