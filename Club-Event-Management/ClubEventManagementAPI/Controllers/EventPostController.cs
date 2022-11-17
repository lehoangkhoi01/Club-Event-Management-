using ApplicationCore;
using ClubEventManagementAPI.Helpers;
using Infrastructure;
using Infrastructure.Services.EventPostServices;
using Infrastructure.Services.EventPostServices.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq;
using System.Security.Claims;

namespace ClubEventManagementAPI.Controllers
{
    public class EventPostController : ODataController
    {
        private readonly EventPostService _eventPostService;
        private readonly UserContextService _userContextService;
        private readonly ClubEventManagementContext _db;

        public EventPostController(ClubEventManagementContext db, EventPostService eventPostService, UserContextService userContextService)
        {
            _eventPostService = eventPostService;
            _db = db;
            _userContextService = userContextService;
        }

        private bool PostExists(int key)
        {
            return _db.EventPosts.Any(p => p.EventPostId == key);
        }

        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IQueryable<EventPost> Get()
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            var result = _eventPostService.GetEventPosts(userContext.IsAdmin, userContext.ClubIds);
            return result;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]CreateEventPostRequest post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Authorize
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            if (_eventPostService.CanModifyPost(userContext.IsAdmin, userContext.OwningClubIds, post.EventId))
            {
                var result = _eventPostService.CreateEventPost(post);
                return result != null ? Accepted(result) : BadRequest("EventStatus is not PUBLISHED or PAST");
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        public IActionResult Put([FromODataUri] int key,[FromBody] UpdateEventPostRequest updateEventPostRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Authorize
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            if (_eventPostService.CanModifyPost(userContext.IsAdmin, userContext.OwningClubIds, updateEventPostRequest.EventId))
            {
                var result = _eventPostService.UpdateEventPost(updateEventPostRequest, key);
                if (result.HasErrors)
                    return BadRequest(result.GetAllErrors());
                return Accepted(result.Result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        public IActionResult Delete([FromODataUri] int key)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            var result = _eventPostService.DeleteEventPost(key, userContext.IsAdmin, userContext.OwningClubIds);
            if (result.HasErrors)
                return BadRequest(result.GetAllErrors());
            if (result.Result == -1)
                return Unauthorized();
            return Ok();
        }
    }
}
