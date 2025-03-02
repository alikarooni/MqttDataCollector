using Microsoft.AspNetCore.Mvc;
using MqttPublisher.Repositories;
using MqttPublisher.Services;

namespace MqttPublisher.Controller
{
    [Route("[controller]")]
    public class BrokersUpdateController : ControllerBase
    {
        private readonly IBrokerService _brokerService;
        private readonly IAppLogger _appLogger;

        public BrokersUpdateController(IBrokerService brokerService, IAppLogger appLogger)        
        {
            _brokerService = brokerService;
            _appLogger = appLogger;
        }

        [HttpGet]
        [Route("ReloadBrokers")]
        public async Task<ActionResult> ReloadBrokers()
        {
            await _brokerService.StartBrokers();

            return Ok(new { message = "Brokers reloaded" });
        }

        [HttpGet]
        [Route("GetAppLogs")]
        public ActionResult GetAppLogs()
        { 
            return Ok(new { Logs = _appLogger.GetLogs()});
        }
    }
}
