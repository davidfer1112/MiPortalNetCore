using Microsoft.AspNetCore.Mvc;
using MiPortal.Models;
using MiPortal.Services;



namespace MiPortal.Controllers
{
    //POST --> api/Email
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] EmailDTO request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
