using ApplicationCore;
using ApplicationCore.Interfaces.Repository;
using Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
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

        public Task DeletePost(int postId)
        => PostDAO.Instance.DeletePost(postId);

        public Task<IEnumerable<EventPost>> GetAllPost()
        => PostDAO.Instance.GetAllPost();

        public Task UpdatePost(EventPost post)
        => PostDAO.Instance.Update(post);

        public async Task<EventPost> GetPostById(int id)
        {
            var dbContext = new ClubEventManagementContext();
            EventPost post = await dbContext.EventPosts
                                            .Include(p => p.StudentAccount)
                                            .ThenInclude(a => a.UserIdentity.Role)
                                            .Include(e => e.StudentAccount.ClubProfiles)
                                            .Include(c => c.Event)
                                            .ThenInclude(b => b.EventType)
                                            .Include(d => d.Event.EventCategory)
                                            .FirstOrDefaultAsync(p => p.EventPostId == id);
            return post;
        }
    }
}
