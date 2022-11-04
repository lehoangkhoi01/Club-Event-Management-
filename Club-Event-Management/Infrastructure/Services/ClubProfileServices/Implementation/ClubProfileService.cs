using ApplicationCore;
using Infrastructure.Services.ClubProfileServices.QueryObject;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Services.GenericPagingQuery;

namespace Infrastructure.Services.ClubProfileServices.Implementation
{
    public class ClubProfileService
    {
        private readonly ClubEventManagementContext _db;

        public ClubProfileService(ClubEventManagementContext db)
        {
            _db = db;
        }

        public PagingResult<ClubProfileListDto> GetClubProfiles(ClubProfileFilterPagingRequest clubProfileFilterPagingRequest)
        {
            return _db.ClubProfiles.AsNoTracking()
                .MapClubProfileToDto(clubProfileFilterPagingRequest.EventSize.Value, clubProfileFilterPagingRequest.StudentProfileSize.Value)
                .FilterAndSorting(clubProfileFilterPagingRequest)
                .Page(clubProfileFilterPagingRequest.PageIndex.Value, clubProfileFilterPagingRequest.PageSize.Value);
        }

        public IStatusGeneric<ClubProfileDetailDto> GetClubProfile(int clubProfileId)
        {
            var returnVal = new StatusGenericHandler<ClubProfileDetailDto>();
            var result = _db.ClubProfiles.AsNoTracking()
                .MapClubProfileToDetailDto(clubProfileId).FirstOrDefault();
            if(result == null)
                return returnVal.AddError("ClubProfile id does not exist", nameof(clubProfileId));
            return returnVal.SetResult(result);
        }

        public IStatusGeneric<ClubProfile> CreateClubProfile(CreateClubProfileRequest createClubProfileRequest)
        {
            var result = new StatusGenericHandler<ClubProfile>();

            //check student inf
            var studentProfileIds = createClubProfileRequest.MemeberInfors.Select(inf => inf.StudentAccountId).ToList();
            var studentProfileCount = _db.StudentAccounts.Select(acc => acc.StudentAccountId).Where(id => studentProfileIds.Contains(id)).Count();
            if(studentProfileCount != studentProfileIds.Count)
            {
                return result.AddError("Some of StudentId does not exist", nameof(createClubProfileRequest.MemeberInfors));
            }
            var student_clubLinks = new List<ClubProfileStudentAccount>();


            //create new club profile
            var newClubProfile = new ClubProfile
            {
                Avatar = createClubProfileRequest.Avatar,
                ClubName = createClubProfileRequest.ClubName,
                CreatedDate = DateTime.Now,
                FoundationDate = createClubProfileRequest.FoundationDate,
                SocialAddress = createClubProfileRequest.SocialAddress,
                Summary = createClubProfileRequest.Summary,
                UpdatedDate = DateTime.Now,
            };

            createClubProfileRequest.MemeberInfors.ForEach(info => student_clubLinks.Add(new ClubProfileStudentAccount
            {
                CanModify = info.CanModify,
                ClubProfile = newClubProfile,
                StudentAccountId = info.StudentAccountId
            }));
            newClubProfile.StudentAccountsLink = student_clubLinks;

            _db.ClubProfiles.Add(newClubProfile);
            _db.SaveChanges();

            return result.SetResult(newClubProfile);
        }

        public IStatusGeneric<ClubProfile> UpdateClubProfile(UpdateClubProfileRequest updateClubProfileRequest, int clubProfileId)
        {
            var result = new StatusGenericHandler<ClubProfile>();

            //check student inf
            var studentProfileIds = updateClubProfileRequest.UpdateMemeberRequests.Select(req => req.StudentProfileId).ToList();
            var studentProfileCount = _db.StudentAccounts.Select(acc => acc.StudentAccountId).Where(id => studentProfileIds.Contains(id)).Count();
            if (studentProfileCount != studentProfileIds.Count)
            {
                return result.AddError("Some of StudentId does not exist", nameof(updateClubProfileRequest.UpdateMemeberRequests));
            }

            //get old profile
            var oldClubProfile = _db.ClubProfiles.Include(club => club.StudentAccountsLink).Where(club => club.ClubProfileId == clubProfileId).FirstOrDefault();
            if(oldClubProfile == null)
            {
                return result.AddError("ClubProfileId does not exist", nameof(clubProfileId));
            }

            //update club profile

            oldClubProfile.Avatar = updateClubProfileRequest.Avatar;
            oldClubProfile.ClubName = updateClubProfileRequest.ClubName;
            oldClubProfile.FoundationDate = updateClubProfileRequest.FoundationDate;
            oldClubProfile.SocialAddress = updateClubProfileRequest.SocialAddress;
            oldClubProfile.Summary = updateClubProfileRequest.Summary;
            oldClubProfile.UpdatedDate = DateTime.Now;

            var oldClubStudentLinks = oldClubProfile.StudentAccountsLink;
            updateClubProfileRequest.UpdateMemeberRequests.ForEach(request =>
            {
                //Add new link
                if (!oldClubStudentLinks.Any(link => link.StudentAccountId == request.StudentProfileId))
                {
                    oldClubStudentLinks.Add(new ClubProfileStudentAccount { CanModify = request.CanModify, StudentAccountId = request.StudentProfileId });
                }
                //Update or Delete link
                else
                {
                    if (request.Remove)
                    {
                        var linkToRemove = _db.ClubProfileStudentAccount.Where(link => link.StudentAccountId == request.StudentProfileId && link.ClubProfileId == clubProfileId).FirstOrDefault();                       
                        _db.ClubProfileStudentAccount.Remove(linkToRemove);
                    }
                    else
                    {
                        oldClubStudentLinks.First(link => link.StudentAccountId == request.StudentProfileId).CanModify = request.CanModify;
                    }
                }
            });

            _db.SaveChanges();

            return result.SetResult(oldClubProfile);
        }


        public List<Tuple<int, bool>> GetClubLinkByStudentAccountId(int studentAccountId)
        {
            return _db.ClubProfileStudentAccount.Where(link => link.StudentAccountId == studentAccountId).Select(link => Tuple.Create(link.ClubProfileId, link.CanModify)).ToList();

        }
    }
}
