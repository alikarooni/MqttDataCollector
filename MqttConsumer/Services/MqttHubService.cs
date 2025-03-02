using Microsoft.AspNetCore.SignalR;
using MqttConsumer.Entities;
using MqttConsumer.SignalRHub;
using Newtonsoft.Json.Linq;

namespace MqttConsumer.Services;


public interface IMqttHubService
{
    Task PushMQttEvent(string projectId, string projectName, string brokerTopic, string token, string value, DateTime eventTime);
}

public class MqttHubService: IMqttHubService
{
    private readonly IHubContext<MqttHub, IMqttHubClient> _hubContext;

    public MqttHubService(IHubContext<MqttHub, IMqttHubClient> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task PushMQttEvent(string projectId, string projectName, string brokerTopic, string token, string value, DateTime eventTime)
    {
        await _hubContext.Clients.All.ReciveMqttEvent(projectId, projectName, brokerTopic, token, value, eventTime);
    }

}
