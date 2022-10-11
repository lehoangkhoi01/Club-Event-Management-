using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repository
{
    public interface IClubProfileRepository
    {
        public Task<ClubProfile> GetClubProfileById(int id);
        public Task<IEnumerable<ClubProfile>> GetAllClubs(int page, int itemPerPage);
        public Task<IEnumerable<ClubProfile>> Search(string searchValue, int page, int itemPerPage);
        public Task Add(ClubProfile clubProfile);
        public Task Update(ClubProfile clubProfile);

    }
}
