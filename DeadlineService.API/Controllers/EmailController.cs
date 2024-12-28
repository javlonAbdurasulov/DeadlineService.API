using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Application.Services.Model;
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
        private readonly IUserService _userService;
        public EmailController(MailgunService mailgun,IUserService userService)
        {
            _userService=userService;   
            _emailSender = mailgun;
        }
        [HttpPost]
        public async Task<ResponseModel<string>> ConfirmEmail(string userEmail)
        {
            var token = Guid.NewGuid().ToString();
            var confirmationLink = $"http://localhost/confirm?email={userEmail}&token={token}";
            await _userService.SendConfirmationEmail(userEmail, confirmationLink);
            return new ResponseModel<string>("Operation is successfuly finally");
        }
    }
}
