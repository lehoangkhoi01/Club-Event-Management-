
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.FirebaseServices.NotificationService
{
    public class NotificationService
    {
        private const string INSTANT_NOTI = "instanceNotification";
        private readonly FirestoreDb _db;

        public NotificationService()
        {
            _db = FirestoreDb.Create("prn-eventmanagement");
        }

        private string getNotificationDateString()
        {
            return String.Format("{0}-{1}", DateTime.Now.Day, DateTime.Now.Month);
        }

        private string getDocumentNameForInstanceNoti(NotificationDto notification)
        {
            return notification.ClubId.HasValue ? "club" + notification.ClubId.ToString() : "event" + notification.EventId.ToString();

        }
        public async Task PublishNotification(NotificationDto notification)
        {
            var datePath = getNotificationDateString();
            var notificationCollectionId = getDocumentNameForInstanceNoti(notification);

            var instanceRef = _db.Collection(INSTANT_NOTI).Document(notificationCollectionId);
            var byDateRef = _db.Collection(datePath).Document(notificationCollectionId);

            Dictionary<string, object> noti = new Dictionary<string, object>();
            noti.Add(DateTime.Now.ToString(), notification);
            await instanceRef.SetAsync(noti, SetOptions.MergeAll);
            await byDateRef.SetAsync(noti, SetOptions.MergeAll);
        }

        public async Task FollowEventAsync(int userId, int eventId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId.ToString());
            var snapShot = await userRef.GetSnapshotAsync();

            Dictionary<string, bool> result;
            snapShot.TryGetValue("followEvents", out result);
            if (result == null)
                result = new Dictionary<string, bool>();
            result.TryAdd(eventId.ToString(), true);
            if(snapShot.Exists)
                await userRef.UpdateAsync("followEvents", result);
            else
            {
                var toAdd = new Dictionary<string, Dictionary<string, bool>>();
                toAdd.Add("followEvents", result);
                await userRef.SetAsync(toAdd);
            }
               
        }

        public async Task UnfollowEventAsync(int userId, int eventId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId.ToString());
            var snapShot = await userRef.GetSnapshotAsync();

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            snapShot.TryGetValue("followEvents", out result);
            if(result != null)
            {
                result.Remove(eventId.ToString());
                await userRef.UpdateAsync("followEvents", result);
            }
        }

        public async Task FollowClubAsync(int userId, int clubId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId.ToString());
            var snapShot = await userRef.GetSnapshotAsync();

            Dictionary<string, bool> result;
            snapShot.TryGetValue("followClubs", out result);
            if (result == null)
            {
                result = new Dictionary<string, bool>();
            }
            result.TryAdd(clubId.ToString(), true);
            if (snapShot.Exists)
                await userRef.UpdateAsync("followClubs", result);
            else
            {
                var toAdd = new Dictionary<string, Dictionary<string, bool>>();
                toAdd.Add("followClubs", result);
                await userRef.SetAsync(toAdd);
            }
        }

        public async Task UnfollowClubAsync(int userId, int clubId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId.ToString());
            var snapShot = await userRef.GetSnapshotAsync();

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            snapShot.TryGetValue("followClubs", out result);
            if(result != null)
            {
                result.Remove(clubId.ToString());
                await userRef.UpdateAsync("followClubs", result);
            }
        }
        public async Task<string[]> GetFollowEventIds(int userId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId.ToString());
            var snapShot = await userRef.GetSnapshotAsync();
            if (!snapShot.Exists)
            {
                return new string[0];
            }

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            snapShot.TryGetValue("followEvents", out result);
            return result.Where(tuple => tuple.Value).Select(tuple => tuple.Key).ToArray();
        }
        public async Task<string[]> GetFollowClubIds(int userId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId.ToString());
            var snapShot = await userRef.GetSnapshotAsync();
            if (!snapShot.Exists)
            {
                return new string[0];
            }

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            snapShot.TryGetValue("followClubs", out result);
            return result.Where(tuple => tuple.Value).Select(tuple => tuple.Key).ToArray();
        }
    }
}
