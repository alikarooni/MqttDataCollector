using MQTTnet.Client;
using MqttPublisher.Entities;
using MqttPublisher.Repositories;

namespace MqttPublisher.Services
{
    public interface IBrokerService
    {
        Task StartBrokers();
    }

    public class BrokerService : IBrokerService
    {
        IProjectRepository _projectRepository;
        private Dictionary<string, IMqttClient> _mqttClients;
        IAppLogger _appLogger;

        public BrokerService(IProjectRepository projectRepository, IAppLogger appLogger)
        {
            _projectRepository = projectRepository;
            _appLogger = appLogger;
            _mqttClients = new Dictionary<string, IMqttClient>();

            StartBrokers().Wait();
        }

        public async Task StartBrokers()
        {
            foreach (var project in _projectRepository.GetProjects())
            {
                if (!_mqttClients.ContainsKey(project.Id))
                {
                   await Register(project);
                    _appLogger.Log($"App mqtt registered: {project.ProjectName}");
                }
            }
        }

        public async Task Register(Project project)
        {
            var registerMqttClient = new RegisterMqttClient(project, _appLogger);
            var mqttClient = await registerMqttClient.RegisterClient();
            _mqttClients.Add(project.Id, mqttClient);
        }
    }
}
