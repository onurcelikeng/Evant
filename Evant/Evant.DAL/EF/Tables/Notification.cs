using System;

namespace Evant.DAL.EF.Tables
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid SpecialId { get; set; }

        public string NotificationType { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }


        // Foreign keys
        public virtual User User { get; set; }
    }
}