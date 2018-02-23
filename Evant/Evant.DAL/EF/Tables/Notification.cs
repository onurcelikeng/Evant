using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Notifications")]
    public class Notification : BaseEntity
    {
        public Guid SenderUserId { get; set; }

        [Required, MaxLength(80)]
        public string Content { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public int NotificationType { get; set; }
        

        public virtual User SenderUser { get; set; }
    }
}
