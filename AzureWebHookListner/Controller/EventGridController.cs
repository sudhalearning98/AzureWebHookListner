using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AzureWebHookListner.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventGridController : ControllerBase
    {
        private readonly ILogger<EventGridController> _logger;

        public EventGridController(ILogger<EventGridController> logger)
        {
            _logger = logger;
        }

        [HttpPost("notify")]
        public IActionResult Notify([FromBody] object eventData)
        {
            // Log or process the incoming event data
            var jsonData = JsonConvert.SerializeObject(eventData);
            _logger.LogInformation($"Received Event: {jsonData}");

            // Process event data for user changes or group membership changes
            dynamic eventDetails = JsonConvert.DeserializeObject(jsonData);
            if (eventDetails != null)
            {
                var operationName = eventDetails[0].data.operationName;

                if (operationName == "Add member to group")
                {
                    string groupName = eventDetails[0].data.targetResources[0].displayName;
                    string userName = eventDetails[0].data.additionalDetails.userPrincipalName;
                    _logger.LogInformation($"User {userName} added to group {groupName}");
                }
                else if (operationName == "Update user")
                {
                    string userPrincipalName = eventDetails[0].data.targetResources[0].userPrincipalName;
                    _logger.LogInformation($"User {userPrincipalName} updated.");
                }
            }

            return Ok("Event received successfully");
        }
        [HttpGet("GetData")]
        public IActionResult GetData(){
            return Ok("Web API");
        }
    }
}
