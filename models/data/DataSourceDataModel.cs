using oodb_mongo_server.models.data;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая источник данных для сервиса
    /// </summary>
    public class DataSourceDataModel : IdDataModel
    {
        public DataSourceDataModel() : base()
        {
        }

        public DataSourceDataModel(string? id, string? name, string? url) : base(id)
        {
            Name = name;
            Url = url;
        }

        public string? Name { get; set; }
        public string? Url { get; set; }
    }
}
