using Infrastructure;
using Infrastructure.Services.EventServices;
using Infrastructure.Services.EventServices.QueryObject;
using Infrastructure.Services.EventServices.Implementation;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ClubEventManagementAPI.Helpers;
using System.Security.Claims;
using Infrastructure.Services.ClubProfileServices.Implementation;
using static Infrastructure.Services.GenericPagingQuery;
using Infrastructure.Services.AccountService.Implementation;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        private readonly UserContextService _userContextService;

        public EventsController(EventService eventService, UserContextService userContextService)
        {
            _eventService = eventService;
            _userContextService = userContextService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GettAllEvents(int? pageIndex, int? pageSize)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            PagingResult<EventListDto> result;

            if (userContext.IsAdmin)
                result = _eventService.GetEvents(new EventFilterPagingRequest(pageIndex, pageSize), true, null);
            else
            {
                result = _eventService.GetEvents(new EventFilterPagingRequest(pageIndex, pageSize), false, userContext.ClubIds);
            }
            return Ok(result);
        }



        [HttpPost("searchOnDraft")]
        [Authorize]
        public IActionResult SearchDraftEvents([FromBody] EventFilterPagingRequest filterPagingRequest)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            PagingResult<EventListDto> result;

            filterPagingRequest.EventStatus = EventStatusEnum.DRAFT;
            if (userContext.IsAdmin)
                result = _eventService.GetDraftEvent(filterPagingRequest, true, null);
            else
            {
                result = _eventService.GetDraftEvent(filterPagingRequest, false, userContext.OwningClubIds);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetEventById(int? postSize, int? activitySize, int id)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);

            var resultEvent = _eventService.GetEvent(postSize.GetValueOrDefault(0), activitySize.GetValueOrDefault(0), id);

            if (resultEvent == null)
                return NotFound();

            //Normal user can access
            if (resultEvent.EventStatus != "DRAFT" && resultEvent.EventStatus != "DELETED" && !resultEvent.IsInternal)
            {
                return Ok(resultEvent);
            }

            //Club member user can access
            if (resultEvent.EventStatus != "DRAFT" && resultEvent.EventStatus != "DELETED" && resultEvent.IsInternal)
            {
                bool canViewInternalEvent = userContext.ClubIds.Any(clubId => resultEvent.ClubInfos.Any(info => info.ClubProfileId == clubId));
                if(canViewInternalEvent)
                    return Ok(resultEvent);
                return Unauthorized();
            }

            //Only editor can access
            bool isEditorOfClubInEvent = userContext.OwningClubIds.Any(clubId => resultEvent.ClubInfos.Any(info => info.ClubProfileId == clubId));
            if (isEditorOfClubInEvent)
                return Ok(resultEvent);

            return Unauthorized();
        }


        [HttpPost("search")]
        [Authorize]
        public IActionResult SearchEvents([FromBody] EventFilterPagingRequest filterPagingRequest)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            var result = _eventService.GetEvents(filterPagingRequest, userContext.IsAdmin, userContext.ClubIds);
            return Ok(result);
        }

        [HttpPost("Category")]
        [Authorize]
        public IActionResult GetEventCategories()
        {
            var result = _eventService.GetEventCategories();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateEvent([FromBody] CreateEventRequest createEventRequest)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            if (!userContext.OwningClubIds.Contains(createEventRequest.OwningClubProfileId))
            {
                return Unauthorized("");
            }

            var result = _eventService.CreateEvent(createEventRequest, createEventRequest.OwningClubProfileId);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted(result.Result.Id);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateEvent([FromBody] UpdateEventRequest updateEventRequest, int id)
        {
            var userContext = _userContextService.GetUserContext(HttpContext.User.Identity as ClaimsIdentity);
            if (!userContext.OwningClubIds.Contains(updateEventRequest.OwningClubProfileId))
            {
                return Unauthorized();

            }
            var result = _eventService.UpdateEvent(updateEventRequest, id, updateEventRequest.OwningClubProfileId);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted(result.Result.Id);

        }

        [HttpGet("TotalEvents")]
        [Authorize]
        public IActionResult GetTotalEvents()
        {
            return Ok(_eventService.GetTotalEvents());
        }

        [HttpGet("PageCount")]
        [Authorize]
        public IActionResult GetTotalPages(int pageSize)
        {
            return Ok(_eventService.GetTotalPages(pageSize));
        }
    }
}
