using MailAPI.Services.IService;
using MailAPI.VmModel;
using Microsoft.AspNetCore.Mvc;

namespace MailAPI.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-bulk")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendBulkEmails([FromForm] EmailRequest request)
        {
            await _emailService.SendBulkEmailsAsync(request);

            return Ok(new
            {
                message = "Emails sent successfully"
            });
        }
    }
}
