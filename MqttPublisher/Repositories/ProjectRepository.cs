using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.EventGrid.Models;
using MongoDB.Driver;
using MqttPublisher.Entities;

namespace MqttPublisher.Repositories
{
    public interface IProjectRepository
    {
        List<Project> GetProjects();
        public List<Project> GetProject(string projectId);
        void AddProject(string domainId, string projectName, string brokerUrl,
            string username, string password, int port, bool useTls, string brokerTopic, string eventGridTopic);
        void RemoveTopic(string projectId);
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly IMongoDBService _mongoDBService;
        private readonly IDomainRepository _domainRepository;

        public ProjectRepository(IMongoDBService mongoDBService, IDomainRepository domainRepository)
        {
            _mongoDBService = mongoDBService;
            _domainRepository = domainRepository;
        }

        public List<Project> GetProjects()
        {
            var projects = _mongoDBService.GetAll<Project>();
            foreach (var project in projects) 
            {
                project.Domain = _domainRepository.GetDomain(project.DomainId);
            }

            return projects;
        }

        public List<Project> GetProject(string projectId)
        {
            return _mongoDBService.GetAll(Builders<Project>.Filter.Eq(x => x.Id, projectId)).ToList();

        }

        public void AddProject(string domainId, string projectName, string brokerUrl,
            string username, string password, int port, bool useTls, string brokerTopic, string eventGridTopic)
        {
            _mongoDBService.Insert(new Project
            {                
                DomainId = domainId,
                ProjectName = projectName,
                BrokerUrl = brokerUrl,
                Username = username,
                Password = password,
                Port = port,
                UseTls = useTls,
                BrokerTopic = brokerTopic,
                EventGridTopic = eventGridTopic,                
            });
        }

        public void RemoveTopic(string projectId)
        {
            _mongoDBService.Delete(Builders<Project>.Filter.Eq(x => x.Id, projectId));
        }
    }
}
