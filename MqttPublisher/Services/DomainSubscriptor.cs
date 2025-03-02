using Azure.Identity;
using Microsoft.Azure.Management.EventGrid;
using Microsoft.Azure.Management.EventGrid.Models;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using System;
using System.Threading.Tasks;
using MqttPublisher.Configurations;

namespace MqttPublisher.Services;

public interface IDomainAndTopicSubscriptor
{
    Task<(string, string)> AddDomain(string domainName);
    Task AddTopic(string domainName, string topicName);
}

public class DomainAndTopicSubscriptor : IDomainAndTopicSubscriptor
{
    private readonly AzureCredentials _azureCredentials;

    public DomainAndTopicSubscriptor(IOptions<AzureCredentials> azureCredentials) 
    {
        _azureCredentials = azureCredentials.Value;
    }

    public async Task<(string, string)> AddDomain(string domainName)
    {
        var credentials = new ClientSecretCredential
            (_azureCredentials.TenantId,
            _azureCredentials.ClientId,
            _azureCredentials.ClientSecret);
        var token = credentials.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://management.azure.com/.default" })).Token;
        var tokenCredentials = new TokenCredentials(token);

        var eventGridManagementClient = new EventGridManagementClient(tokenCredentials)
        {
            SubscriptionId = _azureCredentials.SubscriptionId
        };

        // Create a domain
        var domain = new Domain
        {
            Location = _azureCredentials.Location
        };
        var newDomain = await eventGridManagementClient.Domains.CreateOrUpdateAsync
            (_azureCredentials.ResourceGroupName,
            domainName, domain);
        Console.WriteLine("Domain created");

        return (newDomain.Endpoint, eventGridManagementClient.Domains.ListSharedAccessKeysAsync(_azureCredentials.ResourceGroupName, domainName).Result.Key1);
    }

    public async Task AddTopic(string domainName, string topicName)
    {
        var credentials = new ClientSecretCredential
            (_azureCredentials.TenantId,
            _azureCredentials.ClientId,
            _azureCredentials.ClientSecret);
        var token = credentials.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://management.azure.com/.default" })).Token;
        var tokenCredentials = new TokenCredentials(token);

        var eventGridManagementClient = new EventGridManagementClient(tokenCredentials)
        {
            SubscriptionId = _azureCredentials.SubscriptionId
        };

        // Create a topic in the domain
        var topic = new DomainTopic();
        await eventGridManagementClient.DomainTopics.CreateOrUpdateAsync
            (_azureCredentials.ResourceGroupName, domainName, topicName);
        Console.WriteLine("Topic created");
    }
}