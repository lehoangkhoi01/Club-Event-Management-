using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;

        public PostService(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(EventPost post)
        {
            await _repository.AddNewPost(post);
        }

        public async Task<IEnumerable<EventPost>> GetAllPost()
        {
            return await _repository.GetAllPost();
        }
    }
}
