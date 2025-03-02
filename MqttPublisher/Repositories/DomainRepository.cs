using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MongoDB.Driver;
using MqttPublisher.Entities;
using MqttPublisher.Services;

namespace MqttPublisher.Repositories
{
    public interface IDomainRepository
    {
        List<Domain> GetDomains();
        Domain? GetDomain(string domainId);
        void AddDomain(string domainName, string domainEndPoint, string domainSecretKey);
        void RemoveTopic(string domainId);
    }

    public class DomainRepository : IDomainRepository
    {
        private readonly IMongoDBService _mongoDBService;
        public DomainRepository(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public List<Domain> GetDomains()
        {
            return _mongoDBService.GetAll<Domain>();
        }

        public Domain? GetDomain(string domainId)
        {
            return _mongoDBService.GetAll(Builders<Domain>.Filter.Gt(x => x.Id, domainId)).FirstOrDefault();
        }

        public void AddDomain(string domainName, string domainEndPoint, string domainSecretKey)
        {
            Domain domain = new Domain
            {
                DomainName = domainName,
                DomainSecretKey = domainEndPoint,
                DomainEndPoint = domainSecretKey,
            };

            _mongoDBService.Insert(domain);
        }

        public void RemoveTopic(string domainId)
        {
            _mongoDBService.Delete(Builders<Domain>.Filter.Eq(x => x.Id, domainId));
        }
    }
}
