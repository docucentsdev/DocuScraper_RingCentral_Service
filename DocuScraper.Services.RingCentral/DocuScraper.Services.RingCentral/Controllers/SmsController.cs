using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using DocuScraper.Services.RingCentral.Models;
using DocuScraper.Services.RingCentral.Components;

namespace DocuScraper.Services.RingCentral.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly ILogger<SmsController> logger;
        private readonly IMessageServiceComponent messageServiceComponent;

        public SmsController(ILogger<SmsController> logger, IMessageServiceComponent messageServiceComponent)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.messageServiceComponent = messageServiceComponent ?? throw new ArgumentNullException(nameof(messageServiceComponent)); 
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SmsRequestDTO smsRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSent = await messageServiceComponent.SendMessage(smsRequest);
                    if (isSent)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while sending message.");
                    return StatusCode(500, ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }// class ends
}
