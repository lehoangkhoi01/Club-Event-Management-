using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Students")]
        public async Task<IEnumerable<StudentAccount>> GetStudentList()
        {
            return await _accountService.GetStudentList();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admins")]
        public async Task<IEnumerable<AdminAccount>> GetAdminList()
        {
            return await _accountService.GetAdminList();
        }
    }
}
