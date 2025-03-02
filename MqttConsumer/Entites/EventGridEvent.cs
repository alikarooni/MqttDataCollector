using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MqttConsumer.Entites;
public class EventGridEvent
{
    public string Id { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public dynamic Data { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime EventTime { get; set; }
    public string MetadataVersion { get; set; } = string.Empty;
    public string DataVersion { get; set; } = string.Empty;
}