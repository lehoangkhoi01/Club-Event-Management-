using ApplicationCore;
using ApplicationCore.Interfaces.Repository;
using Infrastructure.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PostRepository : IPostRepository
    {
        public Task AddNewPost(EventPost post)
        => PostDAO.Instance.AddNew(post);

        public Task<IEnumerable<EventPost>> GetAllPost()
        => PostDAO.Instance.GetAllPost();
    }
}
