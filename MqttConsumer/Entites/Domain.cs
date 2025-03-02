using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MqttConsumer.Entities
{
    public class Domain
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DomainName { get; set; } = string.Empty;
        public string DomainEndPoint { get; set; } = string.Empty;
        public string DomainSecretKey { get; set; } = string.Empty;
    }
}
