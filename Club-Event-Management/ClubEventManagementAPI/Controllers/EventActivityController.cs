using ApplicationCore;
using ClubEventManagementAPI.Helpers;
using Infrastructure;
using Infrastructure.Services.EventActivityServices;
using Infrastructure.Services.EventPostServices.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq;
using System.Security.Claims;

namespace ClubEventManagementAPI.Controllers
{
    public class EventActivityController : ODataController
    {

        private readonly ClubEventManagementContext _db;
        private readonly EventActivityService _eventActivityService;
        private readonly UserContextService _userContextService;

        public EventActivityController(ClubEventManagementContext db, EventActivityService eventActivityService, UserContextService userContextService)
        {
            _db = db;
            _eventActivityService = eventActivityService;
            _userContextService = userContextService;
        }

        [EnableQuery]
        [Authorize]
        public IQueryable<EventActivity> Get()
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            return _eventActivityService.GetEventActivities(userContext.IsAdmin, userContext.ClubIds);
        }

        [Authorize]
        public IActionResult Post(CreateEventActivityRequest activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Authorize
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            if (_eventActivityService.CanModifyActivity(userContext.IsAdmin, userContext.OwningClubIds, activity.EventId))
            {
                var result = _eventActivityService.CreateEventActivity(activity);
                return result != null ? Accepted(result) : BadRequest("EventStatus is not PUBLISHED or PAST");
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        public IActionResult Patch([FromODataUri] int key, [FromBody] UpdateEventActivityRequest updateEventActivityRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Authorize
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            if (_eventActivityService.CanModifyActivity(userContext.IsAdmin, userContext.OwningClubIds, updateEventActivityRequest.EventId))
            {
                var result = _eventActivityService.UpdateEventActivity(updateEventActivityRequest, key);
                if (result.HasErrors)
                    return BadRequest(result.GetAllErrors());
                return Accepted(result.Result);
            }
            else
            {
                return Unauthorized();
            }
        }

        public IActionResult Delete([FromODataUri] int key)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            var result = _eventActivityService.DeleteEventActivity(key, userContext.IsAdmin, userContext.OwningClubIds);
            if (result.HasErrors)
                return BadRequest(result.GetAllErrors());
            if (result.Result == -1)
                return Unauthorized();
            return Ok();
        }
    }
}
