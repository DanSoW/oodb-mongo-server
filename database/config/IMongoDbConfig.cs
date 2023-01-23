namespace oodb_mongo_server.database.config
{
    public interface IMongoDbConfig
    {
        string? Database { get; set; }
        string? Host { get; set; }
        int? Port { get; set; }
        string? User { get; set; }
        string? Password { get; set; }
        string? ConnectionString { get; }
    }
}
