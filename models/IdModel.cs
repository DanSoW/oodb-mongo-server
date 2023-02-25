using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace oodb_mongo_server.models
{
    /// <summary>
    /// Абстрактный класс модели входных данных.
    /// Используется для обобщения типов входных данных используемых внутри сервиса
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
