using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MqttPublisher.Entities
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
