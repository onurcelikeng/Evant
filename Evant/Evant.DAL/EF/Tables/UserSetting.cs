using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("UserSettings")]
    public class UserSetting : BaseEntity
    {
        public Guid UserId { get; set; }

        [Required]
        [DefaultValue("light")]
        public string Theme { get; set; }

        [Required]
        [DefaultValue("tr")]
        public string Language { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsFriendshipNotif { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsCommentNotif { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsEventNewComerNotif { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsEventUpdateNotif { get; set; }


        // Foreign keys
        public virtual User User { get; set; }
    }
}
