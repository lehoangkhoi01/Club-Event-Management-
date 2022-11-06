using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.FirebaseServices.NotificationService
{
    [FirestoreData]
    public class NotificationDto
    {
        [FirestoreProperty]
        public int? ClubId { get; set; }
        [FirestoreProperty]
        public string ClubName { get; set; } = string.Empty;
        [FirestoreProperty]
        public int? EventId { get; set; }
        [FirestoreProperty]
        public string Eventname { get; set; } = string.Empty;
        [FirestoreProperty]
        public int SubjectId { get; set; }
        [FirestoreProperty]
        public string SubjectName { get; set; } = string.Empty;
        [FirestoreProperty]
        public string SubjectType { get; set; } = string.Empty;
        [FirestoreProperty]
        public string ActionType { get; set; } = string.Empty;

    }

    public enum SubjectType
    {
       POST, EVENT, ACTIVITY, PROFILE
    }

    public enum ActionType
    {
        UPDATE, CREATE, CANCEL, HAPPENING, SOON
    }
}
