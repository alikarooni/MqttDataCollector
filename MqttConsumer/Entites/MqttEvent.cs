using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MqttConsumer.Entites;
public class MqttEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string TopicId { get; set; } = string.Empty; 
    public string ProjectId { get; set; } = string.Empty;
    public string DomainTopic { get; set; } = string.Empty;
    public string BrokerTopic { get; set; } = string.Empty;
    public string EventTopic { get; set; } = string.Empty;
    public string EventValue { get; set; } = string.Empty;
    public DateTime EventTime { get; set; }
}