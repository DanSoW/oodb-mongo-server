namespace oodb_mongo_server.models.data
{
    /// <summary>
    /// Абстрактный класс модели выходных данных.
    /// Используется для обобщения типов выходных данных для пользователя
    /// </summary>
    public abstract class IdDataModel
    {
        public IdDataModel()
        {
        }

        public IdDataModel(string? id)
        {
            Id = id;
        }

        public string? Id { get; set; }
    }
}
