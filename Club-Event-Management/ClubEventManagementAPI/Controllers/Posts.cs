using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<EventPost>> Post(EventPost post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            EventPost p = new EventPost
            {
                Content = post.Content,
                Picture = post.Picture,
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
        public async Task<IEnumerable<EventPost>> Get()
        {
            return await _service.GetAllPost();
        }
        [HttpDelete]
        [Authorize(Roles ="Student")]
        public async Task<ActionResult<EventPost>> Delete(int postId)
        {
            await _service.Delete(postId);
            return Ok();
        }
        
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<EventPost>> Put(EventPost post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            EventPost p = new EventPost
            {
                Content = post.Content,
                Picture = post.Picture,
                UpdatedDate = System.DateTime.Now,
                StudentAccountId = post.StudentAccountId,
                EventId = post.EventId,

            };

            await _service.Update(p);
            return Ok(p);
        }
    }
}
