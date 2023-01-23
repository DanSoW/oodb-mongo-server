namespace oodb_mongo_server.models.data
{
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
