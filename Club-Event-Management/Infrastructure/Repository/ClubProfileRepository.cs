using ApplicationCore;
using ApplicationCore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ClubProfileRepository : IClubProfileRepository
    {
        public async Task Add(ClubProfile clubProfile)
        {
            var dbContext = new ClubEventManagementContext();
            await dbContext.AddAsync(clubProfile);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClubProfile>> GetAllClubs(int page, int itemPerPage)
        {
            var dbContext = new ClubEventManagementContext();
            IEnumerable<ClubProfile> clubs = await dbContext.ClubProfiles
                                                        .Include(c => c.Events)
                                                        .Include(c => c.StudentAccounts)
                                                        .Skip((page - 1) * itemPerPage)
                                                        .Take(itemPerPage)
                                                        .ToListAsync();
            return clubs;
        }

        public async Task<ClubProfile> GetClubProfileById(int id)
        {
            var dbContext = new ClubEventManagementContext();
            return await dbContext.ClubProfiles.Include(c => c.Events)
                                                .Include(c => c.StudentAccounts)
                                                .FirstOrDefaultAsync(c => c.ClubProfileId == id);
        }

        public async Task<IEnumerable<ClubProfile>> Search(string searchValue, int page, int itemPerPage)
        {
            var dbContext = new ClubEventManagementContext();
            IEnumerable<ClubProfile> clubs = await dbContext.ClubProfiles
                                                            .Include(c => c.Events)
                                                            .Include(c => c.StudentAccounts)
                                                            .Where(c => c.ClubName.ToLower().Contains(searchValue.ToLower()))
                                                            .ToListAsync();
            clubs = clubs.Skip((page - 1) * itemPerPage).Take(itemPerPage);
            return clubs;
                                                    
        }

        public async Task Update(ClubProfile clubProfile)
        {
            var dbContext = new ClubEventManagementContext();
            dbContext.Entry(clubProfile).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
