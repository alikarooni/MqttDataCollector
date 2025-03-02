using Azure.Messaging.EventGrid;
using MqttConsumer.Entites;
using MqttConsumer.Entities;
using MqttConsumer.Repositories;

namespace MqttConsumer.Services;

public interface IMqttEventProcessorService
{
    void ProcessEventGridEvent(Entites.EventGridEvent eventGridEvent);
    List<Entites.EventGridEvent> GetMqttEvents();
}

public class MqttEventProcessorService : IMqttEventProcessorService
{
    private readonly IMqttEventRepository _mqttRepository;
    private readonly IMqttHubService _mqttHubService;
    private readonly List<Entites.EventGridEvent> _eventEvents = new List<Entites.EventGridEvent>();

    public MqttEventProcessorService(IMqttEventRepository mqttRepository, IMqttHubService mqttHubService)
    {
        _mqttRepository = mqttRepository;
        _mqttHubService = mqttHubService;
    }

    public void ProcessEventGridEvent(Entites.EventGridEvent eventGridEvent)
    {
        _eventEvents.Insert(0, eventGridEvent);
        if (_eventEvents.Count > 20)
        {
            _eventEvents.RemoveAt(_eventEvents.Count - 1);
        }

        string topicId = eventGridEvent.Id;
        string projectId = eventGridEvent.Data.ProjectId;
        string projectName = eventGridEvent.Data.ProjectName;
        string domainTopic = eventGridEvent.Topic;
        string brokerTopic = eventGridEvent.Data.BrokerTopic;
        string eventTopic = eventGridEvent.Data.EventTopic;
        string eventValue = eventGridEvent.Data.EventValue;
        DateTime eventTime = eventGridEvent.EventTime;

        _mqttRepository.AddMqttEvent(topicId, projectId, domainTopic, brokerTopic, eventTopic, eventValue, eventTime);
        _mqttHubService.PushMQttEvent(projectId, projectName, brokerTopic, eventTopic, eventValue, eventTime);
    }

    public List<Entites.EventGridEvent> GetMqttEvents()
    {
        return _eventEvents;
    }
}