using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using ClubEventManagementAPI.ViewModels;
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
    //POST API CREATE
        [HttpPost("Create")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<EventPostViewModel>> Post(EventPostViewModel post)
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

        //DELETE
        [HttpDelete("Delete")]
        [Authorize(Roles ="Student")]
        public async Task<ActionResult<EventPost>> Delete(int postId)
        {
            if (_service.GetPostById(postId) == null)
            {
                return NotFound();
            }
            await _service.Delete(postId);
            return Ok();
        }
        
        //PUT API UPDATE
        [HttpPut("Update")]
        [Authorize]
        public async Task<ActionResult<EventPostViewModel>> Put(EventPostViewModel post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            EventPost oldPost = await _service.GetPostById(post.EventPostId);
            if (oldPost == null)
            {
                return NotFound();
            }
            EventPost p = new EventPost
            {
                Content = post.Content,
                Picture = post.Picture,
                CreatedDate = oldPost.CreatedDate,
                UpdatedDate = System.DateTime.Now,
                StudentAccountId = oldPost.StudentAccountId,
                //EventId = oldPost.EventId,

            };

            await _service.Update(p);
            return Ok(p);
        }
    }
}
