using MongoDB.Driver;
using oodb_mongo_server.database.config;
using oodb_project.models;

namespace oodb_mongo_server.database.context
{
    /// <summary>
    /// Класс, реализующий интерфейс контекста MongoDB
    /// </summary>
    public class DbContext : IDbContext
    {
        private readonly IMongoDatabase _db;
        public DbContext(IMongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }

        public IMongoCollection<AdminModel>? AdminList => _db.GetCollection<AdminModel>("AdminList");
        public IMongoCollection<DataSourceModel>? DataSourceList => _db.GetCollection<DataSourceModel>("DataSourceList");
        public IMongoCollection<HostModel>? HostList => _db.GetCollection<HostModel>("HostList");
        public IMongoCollection<HostServiceModel>? HostServiceList => _db.GetCollection<HostServiceModel>("HostServiceList");
        public IMongoCollection<MonitorAppModel>? MonitorAppList => _db.GetCollection<MonitorAppModel>("MonitorAppList");
        public IMongoCollection<ServiceModel>? ServiceList => _db.GetCollection<ServiceModel>("ServiceList");
    }
}
