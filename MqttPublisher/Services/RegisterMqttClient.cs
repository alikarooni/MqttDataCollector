using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Client;
using MQTTnet;
using MqttPublisher.Entities;
using MqttPublisher.Repositories;

namespace MqttPublisher.Services
{
    public interface IRegisterMqttClient
    {
        Task<IMqttClient> RegisterClient();
    }

    public class RegisterMqttClient : IRegisterMqttClient
    {
        private readonly MqttFactory _mqttFactory;
        private readonly IEventGridPublisher _eventGridHandler;
        private readonly Project _project;
        private readonly IAppLogger _appLogger;

        public RegisterMqttClient(Project project, IAppLogger appLogger) 
        {
            _mqttFactory = new MqttFactory();
            _eventGridHandler = new EventGridPublisher(project);
            _project = project;
            _appLogger = appLogger;
        }

        public async Task<IMqttClient> RegisterClient()
        {
            // Create an MQTT client
            var mqttClient = _mqttFactory.CreateMqttClient();
            var mqttClientOptions = CreateMqttClientOptionsBuilder(_project);

            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
            await mqttClient.ConnectAsync(mqttClientOptions);

            mqttClient.DisconnectedAsync += MqttClient_DisconnectedAsync;

            // Subscribe to the MQTT topic
            await mqttClient.SubscribeAsync(_project.BrokerTopic);

            return mqttClient;
        }

        private Task MqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            RegisterClient().Wait();
            return Task.CompletedTask;
        }

        private async Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var topic = arg.ApplicationMessage.Topic;
            var value = arg.ApplicationMessage.ConvertPayloadToString();

            _appLogger.Log($"Publishing mqtt event: {topic}:{value}");
            await _eventGridHandler.SendAsync(topic, value);
        }

        private MqttClientOptions CreateMqttClientOptionsBuilder(Project project)
        {
            if (project.Port == -1)
            {
                return new MqttClientOptionsBuilder()
                        .WithTcpServer(project.BrokerUrl)
                        .WithCredentials(project.Username, project.Password)
                        .WithTlsOptions(new MqttClientTlsOptions { UseTls = true })
                        .Build();
            }
            else
            {
                return new MqttClientOptionsBuilder()
                        .WithTcpServer(project.BrokerUrl, project.Port)
                        .WithCredentials(project.Username, project.Password)
                        .WithTlsOptions(new MqttClientTlsOptions { UseTls = true })
                        .Build();
            }
        }
    }
}
