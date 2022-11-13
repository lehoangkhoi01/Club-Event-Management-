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
    public class StudentAccountController : ODataController
    {
        private readonly ClubEventManagementContext _db;
        private readonly AccountService _service;
        private readonly UserContextService _userContextService;


        public StudentAccountController(ClubEventManagementContext db, AccountService service, UserContextService userContextService)
        {
            _db = db;
            _service = service;
            _userContextService = userContextService;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<StudentAccount> Get()
        {
            return _db.StudentAccounts;
        }

        [HttpGet("api/Student")]
        public IActionResult GetStudentAccountByEmail(string email)
        {
            StudentAccount student = _service.GetStudentAccountFromEmail(email);
            if (student != null)
            {
                return Ok(student);
            }
            
            return NotFound();
        }

        [Authorize(Roles = "Student")]
        public IActionResult Post(CreateStudentAccountRequest request)
        {
            var accountInfo = _userContextService.GetIdentity(HttpContext.User.Identity as ClaimsIdentity);
            var result = _service.CreateStudenAccount(request, accountInfo.Email);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted(result.Result);
        }

        [Authorize(Roles = "Student")]
        public IActionResult Put([FromBody] CreateStudentAccountRequest request, [FromODataUri] int key)
        {
            var accountInfo = _userContextService.GetIdentity(HttpContext.User.Identity as ClaimsIdentity);

            var result = _service.UpdateStudenAccount(request, key, accountInfo.Email);
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted(result.Result);
        }
    }
}
