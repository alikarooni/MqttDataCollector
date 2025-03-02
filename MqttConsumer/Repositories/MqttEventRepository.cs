using MongoDB.Driver;
using MqttConsumer.Entites;
using MqttConsumer.Entities;

namespace MqttConsumer.Repositories
{
    public interface IMqttEventRepository
    {
        List<MqttEvent> GetMqttEvents();
        public List<MqttEvent> GetMqttEven(string mqttEventId);
        void AddMqttEvent(string topicId, string projectId, string domainTopic,
            dynamic brokerTopic, string eventTopic, string eventValue, DateTime eventTime);
        void RemoveMqttEvent(string mqttEventId);
    }

    public class MqttEventRepository : IMqttEventRepository
    {
        private readonly IMongoDBService _mongoDBService;

        public MqttEventRepository(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public List<MqttEvent> GetMqttEvents()
        {
            return _mongoDBService.GetAll<MqttEvent>();
        }

        public List<MqttEvent> GetMqttEven(string mqttEventId)
        {
            return _mongoDBService.GetAll(Builders<MqttEvent>.Filter.Eq(x => x.Id, mqttEventId)).ToList();

        }

        public void AddMqttEvent(string topicId, string projectId, string domainTopic,
            dynamic brokerTopic, string eventTopic, string eventValue, DateTime eventTime)
        {
            MqttEvent mqttEvent = new MqttEvent
            {
                TopicId = topicId,
                ProjectId = projectId,
                DomainTopic = domainTopic,
                BrokerTopic = brokerTopic,
                EventTopic = eventTopic,
                EventTime = eventTime,
                EventValue = eventValue,
            };

            _mongoDBService.Insert(mqttEvent);
        }

        public void RemoveMqttEvent(string mqttEventId)
        {
            _mongoDBService.Delete(Builders<MqttEvent>.Filter.Eq(x => x.Id, mqttEventId));
        }
    }
}
