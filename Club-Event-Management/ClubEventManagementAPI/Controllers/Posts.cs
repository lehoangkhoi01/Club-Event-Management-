using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Posts : ControllerBase
    {
        private readonly IPostService _service;
        public Posts(IPostService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<EventPost>> Post(EventPost post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            EventPost p = new EventPost
            {
                Content = post.Content,
                Picture = post.Content,
                CreatedDate = System.DateTime.Now,
                UpdatedDate = System.DateTime.Now,
                StudentAccountId = post.StudentAccountId,
                EventId = post.EventId,

            };

            await _service.Add(p);
            return Ok(p);
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<IEnumerable<EventPost>> GetPosts()
        {
            return await _service.GetAllPost();
        }
    }
}
