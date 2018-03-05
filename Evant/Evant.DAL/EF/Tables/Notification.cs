using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Notifications")]
    public class Notification : BaseEntity
    {
        public Guid ReceiverUserId { get; set; }

        public Guid? SenderUserId { get; set; }

        public Guid? EventId { get; set; }

        public Guid? CommentId { get; set; }

        [Required, MaxLength(120)]
        public string Content { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public int NotificationType { get; set; }


        public virtual User SenderUser { get; set; }

        public virtual Event Event { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
