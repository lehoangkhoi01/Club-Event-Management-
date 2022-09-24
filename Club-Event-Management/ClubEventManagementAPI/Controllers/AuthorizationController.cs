using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces.Services;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ApplicationCore.Interfaces.Services.IAuthorizationService _service;
        public AuthorizationController(ApplicationCore.Interfaces.Services.IAuthorizationService service)
        {
            _service = service;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] string email)
        {
            var user = await _service.Login(email);
            if(user != null)
            {
                var token = _service.GenerateToken(user);
                return Ok(token);
            }
            return NotFound();
        }
    }
}
