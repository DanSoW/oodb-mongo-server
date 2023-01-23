using MongoDB.Driver;
using oodb_project.models;

namespace oodb_mongo_server.database.context
{
    public interface IDbContext
    {
        IMongoCollection<AdminModel>? AdminList { get; }
        IMongoCollection<DataSourceModel>? DataSourceList { get; }
        IMongoCollection<HostModel>? HostList { get; }
        IMongoCollection<HostServiceModel>? HostServiceList { get; }
        IMongoCollection<MonitorAppModel>? MonitorAppList { get; }
        IMongoCollection<ServiceModel>? ServiceList { get; }
    }
}
