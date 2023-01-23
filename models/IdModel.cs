using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace oodb_mongo_server.models
{
    /// <summary>
    /// Абстрактный класс для всех моделей
    /// </summary>
    public abstract class IdModel
    {
        public IdModel()
        {
            Id = ObjectId.GenerateNewId();
        }

        public IdModel(ObjectId? id)
        {
            Id = id;
        }

        [BsonId]
        public ObjectId? Id { get; set; }
    }
}
