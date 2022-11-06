using ClubEventManagementAPI.ViewModels;
using Infrastructure.Services.FirebaseServices.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly NotificationService _notiService;

        public TestController(NotificationService notiService)
        {
            _notiService = notiService;
        }

        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.FullName}, you are an {currentUser.RoleName}");
        }

        [HttpGet("Students")]
        [Authorize(Roles = "Student")]
        public IActionResult SellersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.FullName}, you are a {currentUser.RoleName}");
        }

        [HttpGet("Noti")]
        public async Task<IActionResult> Notify()
        {
            await _notiService.PublishNotification(new NotificationDto
            {
                ActionType = ActionType.UPDATE.ToString(),
                ClubId = 1,
                SubjectType = SubjectType.PROFILE.ToString(),
                SubjectId = 1
            });
            return Ok();
        }

        [HttpGet("Public/{id}")]
        public async Task<IActionResult> Public(int id)
        {
            return Ok(await _notiService.GetFollowEventIds(id));
        }

        [HttpGet("Follow/{eventId}/{studentId}")]
        public async Task<IActionResult> Public(int eventId, int studentId)
        {
            await _notiService.FollowEventAsync(studentId, eventId);
            return Ok();
        }

        [HttpGet("Unfollow/{eventId}/{studentId}")]
        public async Task<IActionResult> Unfollow(int eventId, int studentId)
        {
            await _notiService.UnfollowEventAsync(studentId, eventId);
            return Ok();
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
                    RoleName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                    FullName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value
                };
            }
            return null;
        }
    }
}
