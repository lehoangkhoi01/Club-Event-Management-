using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using ClubEventManagementAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClubEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;
        private readonly IMapper _mapper;
        public PostController(IPostService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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
            EventPost eventPost = _mapper.Map<EventPost>(post);
            await _service.Add(eventPost);
            return Ok(eventPost);
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
                EventId = oldPost.EventId,

            };

            await _service.Update(p);
            return Ok(p);
        }
    }
}
