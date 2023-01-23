using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using oodb_mongo_server.models;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая источник данных для сервиса
    /// </summary>
    public class DataSourceModel : IdModel
    {
        public DataSourceModel() : base()
        {
        }

        public DataSourceModel(ObjectId? id, string? name, string? url) : base(id)
        {
            Id = id;
            Name = name;
            Url = url;
        }

        public DataSourceModel(string? name, string? url) : base()
        {
            Name = name;
            Url = url;
        }

        public string? Name { get; set; }
        public string? Url { get; set; }
    }
}
