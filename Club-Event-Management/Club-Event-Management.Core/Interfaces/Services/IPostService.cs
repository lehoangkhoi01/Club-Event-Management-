using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IPostService
    {
        public Task Add(EventPost post);
        public Task<IEnumerable<EventPost>> GetAllPost();
        public Task Delete(int postId);
        public Task Update(EventPost post);
        public Task<EventPost> GetPostById(int postId);
    }
}
