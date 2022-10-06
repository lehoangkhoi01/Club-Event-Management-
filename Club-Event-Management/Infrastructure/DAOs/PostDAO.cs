using ApplicationCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAOs
{
    public class PostDAO
    {
        private readonly ClubEventManagementContext _context;
        private static PostDAO instance;
        private static readonly object instanceLock = new object();

        private PostDAO()
        {
        }

        public static PostDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PostDAO();
                    }
                }
                return instance;
            }
        }
        //-------------------------------

        public async Task AddNew(EventPost post)
        {
            var dbContext = new ClubEventManagementContext();
            await dbContext.EventPosts.AddAsync(post);
            await dbContext.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<EventPost>> GetAllPost()
        {
            var dbContext = new ClubEventManagementContext();
            return await dbContext.EventPosts.Include(p => p.StudentAccount).ThenInclude(a => a.ClubProfiles).Include(c=> c.Event).ThenInclude(b=>b.ClubProfiles).ToListAsync();
        }

        public async Task DeletePost(int postId)
        {
            EventPost post = new EventPost();
            var dbContext = new ClubEventManagementContext();
            post = dbContext.EventPosts.FirstOrDefault(p => p.EventPostId == postId);
            dbContext.EventPosts.Remove(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(EventPost post)
        {
            var dbContext = new ClubEventManagementContext();
            dbContext.EventPosts.Update(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task<EventPost> GetPostById(int id)
        {
            var dbContext = new ClubEventManagementContext();
            return await dbContext.EventPosts.FindAsync(id);
        }
    }
}
