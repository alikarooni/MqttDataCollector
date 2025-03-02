using Microsoft.AspNetCore.Mvc;
using MqttConsumer.Services;

namespace MqttConsumer;

[Route("[controller]")]
public class MqttEventRetrieval : ControllerBase
{
    private readonly IMqttEventProcessorService _mqttEventRepository;

    public MqttEventRetrieval(IMqttEventProcessorService mqttEventRepository)
    {
        _mqttEventRepository = mqttEventRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_mqttEventRepository.GetMqttEvents());
    }
}