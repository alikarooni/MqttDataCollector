using Microsoft.AspNetCore.Mvc;
using MqttConsumer.Entites;
using MqttConsumer.Services;

namespace MqttConsumer;

[Route("[controller]")]
public class EventReceiverController : ControllerBase
{
    private readonly IMqttEventProcessorService _mqttEventProcessorService;
    private readonly string[] EVENT_TYPES = { "Microsoft.EventGrid.SubscriptionValidationEvent", "SubscriptionValidationEvent" };

    public EventReceiverController(IMqttEventProcessorService mqttEventProcessorService)
    {
        _mqttEventProcessorService = mqttEventProcessorService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] EventGridEvent[] mqttEvents)
    {
        foreach (EventGridEvent mqttEvent in mqttEvents)
        {
            // Handle system events
            if (EVENT_TYPES.Contains(mqttEvent.EventType)) //== "Microsoft.EventGrid.SubscriptionValidationEvent"                 mqttEvent.EventType == "SubscriptionValidationEvent")
            {
                // Do any additional validation (as required) and then return back the below response
                var responseData = new
                {
                    ValidationResponse = mqttEvent.Data.validationCode
                };

                return new OkObjectResult(responseData);
            }

            _mqttEventProcessorService.ProcessEventGridEvent(mqttEvent);
        }

        return new OkObjectResult("We are cool!");
    }

    public IActionResult Get()
    {
        return Ok("");
    }
}
