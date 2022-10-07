using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repository
{
    public interface IPostRepository
    {
        public Task AddNewPost(EventPost post);
        public Task<IEnumerable<EventPost>> GetAllPost();
        public Task DeletePost(int postId);
        public Task UpdatePost(EventPost post);
        public Task <EventPost> GetPostById(int id);

    }
}
