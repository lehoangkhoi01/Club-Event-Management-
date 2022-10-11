using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationCore;
using Infrastructure;
using ApplicationCore.Interfaces.Services;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAccountsController : ControllerBase
    {
        private readonly ClubEventManagementContext _context;
        private readonly IAccountService _accountService;

        public StudentAccountsController(ClubEventManagementContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        // GET: api/StudentAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentAccount>>> GetStudentAccounts()
        {
            return await _context.StudentAccounts.ToListAsync();
        }

        // GET: api/StudentAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentAccount>> GetStudentAccount(int id)
        {
            var studentAccount = await _context.StudentAccounts.FindAsync(id);

            if (studentAccount == null)
            {
                return NotFound();
            }

            return studentAccount;
        }

        // PUT: api/StudentAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentAccount(int id, StudentAccount studentAccount)
        {
            if (id != studentAccount.StudentAccountId)
            {
                return BadRequest();
            }

            _context.Entry(studentAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StudentAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentAccount>> PostStudentAccount(StudentAccount studentAccount)
        {
            _context.StudentAccounts.Add(studentAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentAccount", new { id = studentAccount.StudentAccountId }, studentAccount);
        }

        // DELETE: api/StudentAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAccount(int id)
        {
            var studentAccount = await _context.StudentAccounts.FindAsync(id);
            if (studentAccount == null)
            {
                return NotFound();
            }

            _context.StudentAccounts.Remove(studentAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentAccountExists(int id)
        {
            return _context.StudentAccounts.Any(e => e.StudentAccountId == id);
        }
    }
}
