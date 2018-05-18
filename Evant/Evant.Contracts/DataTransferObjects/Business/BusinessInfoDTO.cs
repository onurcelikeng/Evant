using System;

namespace Evant.Contracts.DataTransferObjects.Business
{
    public class BusinessInfoDTO
    {
        public string Type { get; set; }

        public bool IsSendNotificationUsers { get; set; }

        public bool IsAgeAnalysis { get; set; }

        public bool IsCommentAnalysis { get; set; }

        public bool IsAttendedUserAnalysis { get; set; }

        public bool IsChatBotSupport { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
