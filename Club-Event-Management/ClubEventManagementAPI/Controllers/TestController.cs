using ClubEventManagementAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Email}, you are an {currentUser.RoleName}");
        }

        [HttpGet("Students")]
        [Authorize(Roles = "Student")]
        public IActionResult SellersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Email}, you are a {currentUser.RoleName}");
        }

        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi, you're on public property");
        }

        private UserViewModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserViewModel
                {
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    RoleName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
