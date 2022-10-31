using ClubEventManagementAPI.Helpers;
using Infrastructure;
using Infrastructure.Services.ClubProfileServices;
using Infrastructure.Services.ClubProfileServices.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubProfileController : ControllerBase
{ 
        private readonly ClubProfileService _service;
        private readonly UserContextService _userContextService;


        public ClubProfileController(UserContextService userContextService, ClubProfileService service)
        {

            _service = service;
            _userContextService = userContextService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllClubProfiles([FromQuery] ClubProfileFilterPagingRequest request)
        {
            return Ok(_service.GetClubProfiles(request));
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
