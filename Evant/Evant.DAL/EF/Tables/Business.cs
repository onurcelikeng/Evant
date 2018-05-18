using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Business")]
    public class Business : BaseEntity
    {
        public Guid UserId { get; set; }

        public string BusinessType { get; set; }

        public bool IsSendNotificationUsers { get; set; } = false;

        public bool IsAgeAnalysis { get; set; } = false;

        public bool IsCommentAnalysis { get; set; } = false;

        public bool IsAttendedUserAnalysis { get; set; } = false;

        public bool IsChatBotSupport { get; set; } = false;

        public DateTime ExpireDate { get; set; } = DateTime.Now;


        public virtual User User { get; set; }
    }
}
