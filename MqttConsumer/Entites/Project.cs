using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MqttConsumer.Entities
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string DomainId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string BrokerUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseTls { get; set; }
        public string BrokerTopic { get; set; } = string.Empty;
        public string EventGridTopic { get; set; } = string.Empty;
        public Domain Domain { get; set; }
    }
}