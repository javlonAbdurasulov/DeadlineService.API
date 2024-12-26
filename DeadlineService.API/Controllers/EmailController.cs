using DeadlineService.Domain.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MyApp.Services;

namespace DeadlineService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController
    {
        private readonly MailgunService _emailSender;
        public EmailController(MailgunService mailgun)
        {
            _emailSender = mailgun;
        }

        public async Task<ResponseModel<>>
    }
}
