using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest;
using MQTTnet;
using MQTTnet.Client;
using MqttPublisher.Entities;
using Newtonsoft.Json.Linq;

namespace MqttPublisher.Services
{
    public interface IEventGridPublisher
    {
        Task SendAsync(string topic, string value);
    }

    public class EventGridPublisher : IEventGridPublisher
    {
        private EventGridPublisherClient eventGridPublisherClient = null;
        private readonly Project _project;

        public EventGridPublisher(Project project) 
        {
            _project = project;
            eventGridPublisherClient = CreateEventGridPublisherClient(_project);
        }

        public async Task SendAsync(string topic, string value)
        {
            if (eventGridPublisherClient == null)
                eventGridPublisherClient = CreateEventGridPublisherClient(_project);

            var topics = topic.Split('/');
            string brokerTopic = topics[0];
            string eventTopic = string.Join('/', topics.Skip(1));

            // Create an Event Grid event
            Azure.Messaging.EventGrid.EventGridEvent mqttEvent = new Azure.Messaging.EventGrid.EventGridEvent(
                subject: "NoSubject",
                eventType: "Event",
                dataVersion: "1.0",
                data: new
                {
                    ProjectId = _project.Id,
                    ProjectName = _project.ProjectName,
                    BrokerTopic = brokerTopic,
                    EventTopic = eventTopic,
                    EventValue = value,
                })
            {
                Topic = _project.EventGridTopic
            };

            // Send the event
            await eventGridPublisherClient.SendEventAsync(mqttEvent);
        }

        private EventGridPublisherClient CreateEventGridPublisherClient(Project project)
        {
            string domainEndpoint = project.Domain.DomainEndPoint;
            string domainAccessKey = project.Domain.DomainSecretKey;

            EventGridPublisherClient client = new EventGridPublisherClient(
                new Uri(domainEndpoint),
                new AzureKeyCredential(domainAccessKey));

            return client;
        }
    }
}