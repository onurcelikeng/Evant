using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("UserSettings")]
    public class UserSetting : BaseEntity
    {
        public Guid UserId { get; set; }

        
        [Required]
        public string Theme { get; set; } = "light";

        [Required]
        public string Language { get; set; } = "tr";


        [Required]
        public bool IsTwoFactorAuthentication { get; set; } = false;

        [Required]
        public bool IsFriendshipNotif { get; set; } = true;

        [Required]
        public bool IsCommentNotif { get; set; } = true;

        [Required]
        public bool IsEventNewComerNotif { get; set; } = true;

        [Required]
        public bool IsEventUpdateNotif { get; set; } = true;

        [Required]
        public bool IsCommentVisiableTimeline { get; set; } = true;

        [Required]
        public bool IsFollowerVisiableTimeline { get; set; } = true;

        [Required]
        public bool IsFollowingVisiableTimeline { get; set; } = true;

        [Required]
        public bool IsCreateEventVisiableTimeline { get; set; } = true;

        [Required]
        public bool IsJoinEventVisiableTimeline { get; set; } = true;


        public virtual User User { get; set; }
    }
}
