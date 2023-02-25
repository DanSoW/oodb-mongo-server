using MongoDB.Driver;
using oodb_project.models;

namespace oodb_mongo_server.database.context
{
    /// <summary>
    /// Интерфейс контекста MongoDB
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Коллекция Admin
        /// </summary>
        IMongoCollection<AdminModel>? AdminList { get; }

        /// <summary>
        /// Коллекция DataSource
        /// </summary>
        IMongoCollection<DataSourceModel>? DataSourceList { get; }

        /// <summary>
        /// Коллекция Host
        /// </summary>
        IMongoCollection<HostModel>? HostList { get; }

        /// <summary>
        /// Коллекция HostService
        /// </summary>
        IMongoCollection<HostServiceModel>? HostServiceList { get; }

        /// <summary>
        /// Коллекция MonitorApp
        /// </summary>
        IMongoCollection<MonitorAppModel>? MonitorAppList { get; }

        /// <summary>
        /// Коллекция Service
        /// </summary>
        IMongoCollection<ServiceModel>? ServiceList { get; }
    }
}
