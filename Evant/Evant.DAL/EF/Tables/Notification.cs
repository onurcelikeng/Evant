using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Notifications")]
    public class Notification : BaseEntity
    {
        public Guid ReceiverUserId { get; set; }

        [Required]
        public Guid CustomId { get; set; }

        [Required, MaxLength(120)]
        public string Content { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public int NotificationType { get; set; }

        [Required]
        public string UserImage { get; set; }

        public string EventImage { get; set; }


        public virtual User ReceiverUser { get; set; }
    }
}
