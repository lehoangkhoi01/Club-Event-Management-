using ClubEventManagementAPI.Helpers;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.ClubProfileServices;
using Infrastructure.Services.ClubProfileServices.Implementation;
using Infrastructure.Services.FirebaseServices.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubProfileController : ControllerBase
{ 
        private readonly ClubProfileService _service;
        private readonly UserContextService _userContextService;
        private readonly NotificationService _notificationService;


        public ClubProfileController(UserContextService userContextService, ClubProfileService service, NotificationService notificationService)
        {

            _service = service;
            _userContextService = userContextService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllClubProfiles([FromQuery] ClubProfileFilterPagingRequest request)
        {
            return Ok(_service.GetClubProfiles(request));
        }

        [HttpGet("follow")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> GetFollowClubsAsync(int? eventSize, int? studentProfileSize, int? pageIndex, int? pageSize)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            var requestPaging = new ClubProfileFilterPagingRequest
            {
                EventSize = eventSize.GetValueOrDefault(0),
                PageSize = pageSize.GetValueOrDefault(5),
                StudentProfileSize = studentProfileSize.GetValueOrDefault(0),
                PageIndex = pageIndex.GetValueOrDefault(0)
            };
            return Ok(await _service.GetFollowClubProfilesAsync(requestPaging,userContext.StudentAccountId.Value));
        }

        [HttpGet("{id}/follow")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> FollowClubAsync(int id)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            var clubToFollow = _service.GetClubProfile(id);
            if (clubToFollow == null)
                return NotFound();
            await _notificationService.FollowClubAsync(userContext.StudentAccountId.Value, id);
            return Ok();
        }

        [HttpGet("{id}/unfollow")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UnfollowClubAsync(int id)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            await _notificationService.UnfollowClubAsync(userContext.StudentAccountId.Value, id);
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetClubProfile(int id)
        {
            var result = _service.GetClubProfile(id);
            if (result.HasErrors)
            {
                return NotFound();
            }
            else
            {
                return Ok(result.Result);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateClubProfile(int id, [FromBody] UpdateClubProfileRequest request)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);

            if(!userContext.OwningClubIds.Contains(id) && !userContext.IsAdmin)
            {
                return Unauthorized();
            }

            var result = _service.UpdateClubProfile(request, id);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }

            return Accepted(result.Result);
        }

        //Only admin
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateClubProfile([FromBody] CreateClubProfileRequest request)
        {
            var result = _service.CreateClubProfile(request);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }

            return Accepted(result.Result);
        }
    }
}
