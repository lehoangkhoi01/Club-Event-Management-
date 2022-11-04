using ApplicationCore;
using ClubEventManagementAPI.Helpers;
using Infrastructure;
using Infrastructure.Services.AccountService;
using Infrastructure.Services.AccountService.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq;
using System.Security.Claims;

namespace ClubEventManagementAPI.Controllers
{
    public class AdminAccountController : ODataController
    {
        private readonly ClubEventManagementContext _db;
        private readonly AccountService _service;
        private readonly UserContextService _userContextService;

        public AdminAccountController(ClubEventManagementContext db, AccountService service, UserContextService userContextService)
        {
            _db = db;
            _service = service;
            _userContextService = userContextService;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "Admin")]
        public IQueryable<AdminAccount> Get()
        {
            return _db.AdminAccounts;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Post(CreateAdminAccountRequest request)
        {
            var accountInfo = _userContextService.GetIdentity(HttpContext.User.Identity as ClaimsIdentity);
            var result = _service.CreateAdminAccount(request, accountInfo.Email);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted(result.Result);
        }

        public IActionResult Put([FromBody] CreateAdminAccountRequest request, [FromODataUri] int key)
        {
            var accountInfo = _userContextService.GetIdentity(HttpContext.User.Identity as ClaimsIdentity);
            var result = _service.UpdateAdminAccount(request, key, accountInfo.Email);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted(result.Result);
        }
    }
}
