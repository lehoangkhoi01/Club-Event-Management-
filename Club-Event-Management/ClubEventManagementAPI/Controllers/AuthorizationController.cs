using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces.Services;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ApplicationCore.Interfaces.Services.IAuthorizationService _service;
        private readonly IAccountService _accountService;
        public AuthorizationController(ApplicationCore.Interfaces.Services.IAuthorizationService service, 
                                        IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await _service.Login(userLogin);
            if(user != null)
            {
                var token = await _service.GenerateToken(user);
                return Ok(token);
            }
            return NotFound();
        }
    }
}
