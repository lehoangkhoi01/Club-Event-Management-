using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices.QueryObject
{
    public static class ClubProfileListDtoFilter
    {
        public static IQueryable<ClubProfileListDto> FilterAndSorting(this IQueryable<ClubProfileListDto> clubProfiles, ClubProfileFilterPagingRequest clubProfileFilterPaging)
        {
            return clubProfiles.OrderByDescending(profile => profile.TotalMember)
                .Where(profile => profile.ClubName.Contains(clubProfileFilterPaging.ClubName));
        }
    }
}
