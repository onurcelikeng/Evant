using System;

namespace Evant.DAL.EF.Tables
{
    public class EventOperation : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid EventId { get; set; }


        // Foreign keys
        public virtual User User { get; set; }

        public virtual Event Event { get; set; }
    }
}