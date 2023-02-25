namespace oodb_mongo_server.database.config
{
    /// <summary>
    /// Интерфейс конфигурации MongoDB
    /// </summary>
    public interface IMongoDbConfig
    {
        /// <summary>
        /// Название базы данных
        /// </summary>
        string? Database { get; set; }

        /// <summary>
        /// Хост
        /// </summary>
        string? Host { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        int? Port { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        string? User { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        string? Password { get; set; }

        /// <summary>
        /// Строка подключения
        /// </summary>
        string? ConnectionString { get; }
    }
}
