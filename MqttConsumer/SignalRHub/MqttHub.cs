using Microsoft.AspNetCore.SignalR;
using MqttConsumer.Entities;
using MqttConsumer.Repositories;
using Newtonsoft.Json.Linq;

namespace MqttConsumer.SignalRHub
{
    public interface IMqttHubClient
    {
        Task GetDomains(List<Domain> domains);
        Task GetProjects(List<Project> projects);
        Task AddDomain(string message);
        Task AddProject(string message);
        Task ReciveMqttEvent(string projectId, string projectName, string brokerTopic, string token, string value, DateTime eventTime);
    }

    public class MqttHub : Hub<IMqttHubClient>
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IProjectRepository _projectRepository;
        public MqttHub(IDomainRepository domainRepository, IProjectRepository projectRepository)
        {
            _domainRepository = domainRepository;
            _projectRepository = projectRepository;
        }

        public async Task GetDomains()
        {
            await Clients.Client(Context.ConnectionId).GetDomains(_domainRepository.GetDomains());
        }

        public async Task GetProjects()
        {
            await Clients.Client(Context.ConnectionId).GetProjects(_projectRepository.GetProjects());
        }

        public async Task AddDomain(string domainName, string domainEndPoint, string domainSecretKey)
        {
            //_domainRepository.AddDomain(domainName, domainEndPoint, domainSecretKey);
            string message = "Domain Added.";
            await Clients.Client(Context.ConnectionId).AddDomain(message);
        }

        public async Task AddProject(string domainId, string projectName, string brokerUrl, string Username, string password,
            string port, bool useTls, string brokerTopic, string eventGridTopic)
        {
            //_projectRepository.AddProject(domainId, projectName, brokerUrl, Username, password, port, useTls, brokerTopic, eventGridTopic);
            string message = "Project Added.";
            await Clients.Client(Context.ConnectionId).AddProject(message);
        }

        public async Task PushNewMqttEvent(string projectId, string projectName, string brokerTopic, string token, string value, DateTime eventTime)
        {
            //_projectRepository.AddProject(domainId, projectName, brokerUrl, Username, password, port, useTls, brokerTopic, eventGridTopic);
            await Clients.Client(Context.ConnectionId).ReciveMqttEvent(projectId, projectName, brokerTopic, token, value, eventTime);
        }
    }
}