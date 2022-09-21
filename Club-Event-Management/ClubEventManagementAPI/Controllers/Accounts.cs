using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Accounts : ControllerBase
    {
        private readonly IAccountService _service;
        public Accounts(IAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<UserIdentity>> Post ()
        {
            UserIdentity user = new UserIdentity
            {
                Email = "khoi@gmail.com",
                RoleId = 1,
                IsLocked = true,
            };
            
            await _service.Add(user);
            return Ok(user);
        }
    }
}
