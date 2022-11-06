using ApplicationCore;
using Infrastructure.Services.ClubProfileServices.QueryObject;
using Infrastructure.Services.FirebaseServices.NotificationService;
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
        private readonly NotificationService _notificationService;

        public ClubProfileService(ClubEventManagementContext db, NotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        public async Task<PagingResult<ClubProfileListDto>> GetFollowClubProfilesAsync(ClubProfileFilterPagingRequest clubProfileFilterPagingRequest, int studentId)
        {
            var clubIds = (await _notificationService.GetFollowClubIds(studentId)).ToList();
            return _db.ClubProfiles.AsNoTracking()
                .Where(club => clubIds.Contains(club.ClubProfileId.ToString()))
                .OrderBy(club => club.ClubProfileId)
                .MapClubProfileToDto(clubProfileFilterPagingRequest.EventSize.Value, clubProfileFilterPagingRequest.StudentProfileSize.Value)
                .Page(clubProfileFilterPagingRequest.PageIndex.Value, clubProfileFilterPagingRequest.PageSize.Value);
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
            var studentEmailsRequest = createClubProfileRequest.MemeberInforMap.Keys;
            var studentProfileEmailMap = _db.StudentAccounts.AsQueryable()
                .Where(student => studentEmailsRequest.Contains(student.UserIdentity.Email))
                .Select(acc => new { acc.StudentAccountId, acc.UserIdentity.Email })
                .ToDictionary(tuple => tuple.Email, tuple => tuple.StudentAccountId);
            if(studentEmailsRequest.Count != studentProfileEmailMap.Count)
            {
                return result.AddError("Some of student's email does not exist", nameof(createClubProfileRequest.MemeberInforMap));
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

            var memberInfoMap = createClubProfileRequest.MemeberInforMap;
            memberInfoMap.Keys.ToList().ForEach(email => student_clubLinks.Add(new ClubProfileStudentAccount
            {
                CanModify = memberInfoMap.GetValueOrDefault(email),
                ClubProfile = newClubProfile,
                StudentAccountId = studentProfileEmailMap.GetValueOrDefault(email)
            })); ;
            newClubProfile.StudentAccountsLink = student_clubLinks;

            _db.ClubProfiles.Add(newClubProfile);
            _db.SaveChanges();

            return result.SetResult(newClubProfile);
        }

        public IStatusGeneric<ClubProfile> UpdateClubProfile(UpdateClubProfileRequest updateClubProfileRequest, int clubProfileId)
        {
            var result = new StatusGenericHandler<ClubProfile>();

            //check student inf
            var studentProfileEmailsRequest = updateClubProfileRequest.UpdateMemeberRequestsMap.Keys;
            var studentProfileEmailMap = _db.StudentAccounts.AsQueryable()
                .Where(student => studentProfileEmailsRequest.Contains(student.UserIdentity.Email))
                .Select(acc => new { acc.StudentAccountId, acc.UserIdentity.Email })
                .ToDictionary(tuple => tuple.Email, tuple => tuple.StudentAccountId);
            if (studentProfileEmailMap.Count != studentProfileEmailsRequest.Count)
            {
                return result.AddError("Some of StudentId does not exist", nameof(updateClubProfileRequest.UpdateMemeberRequestsMap));
            }
            var emailAccountIdMap = new Dictionary<string, int>();

            //get old profile
            var oldClubProfile = _db.ClubProfiles.Include(club => club.StudentAccountsLink)
                .ThenInclude(link => link.StudentAccount).ThenInclude(acc => acc.UserIdentity)
                .Where(club => club.ClubProfileId == clubProfileId).FirstOrDefault();
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
            var requestMap = updateClubProfileRequest.UpdateMemeberRequestsMap;
            requestMap.Keys.ToList().ForEach(email =>
            {
                //Add new link
                if (!oldClubStudentLinks.Any(link => link.StudentAccountId == studentProfileEmailMap.GetValueOrDefault(email))
                    && !requestMap.GetValueOrDefault(email).Remove)
                {
                    oldClubStudentLinks.Add(new ClubProfileStudentAccount { CanModify = requestMap.GetValueOrDefault(email).CanModify, StudentAccountId = studentProfileEmailMap.GetValueOrDefault(email) });
                }
                //Update or Delete link
                else
                {
                    if (requestMap.GetValueOrDefault(email).Remove)
                    {
                        var linkToRemove = _db.ClubProfileStudentAccount.AsQueryable().Where(link => link.StudentAccountId == studentProfileEmailMap.GetValueOrDefault(email) && link.ClubProfileId == clubProfileId).FirstOrDefault();                       
                        _db.ClubProfileStudentAccount.Remove(linkToRemove);
                    }
                    else
                    {
                        oldClubStudentLinks.First(link => link.StudentAccountId == studentProfileEmailMap.GetValueOrDefault(email)).CanModify = requestMap.GetValueOrDefault(email).CanModify;
                    }
                }
            });

            _db.SaveChanges();

            return result.SetResult(oldClubProfile);
        }


        public List<Tuple<int, bool>> GetClubLinkByStudentAccountId(int studentAccountId)
        {
            return _db.ClubProfileStudentAccount.AsQueryable().Where(link => link.StudentAccountId == studentAccountId).Select(link => Tuple.Create(link.ClubProfileId, link.CanModify)).ToList();

        }
    }
}
