using System;

namespace Evant.DAL.EF.Tables
{
    public class EventTag : BaseEntity
    {
        public Guid TagId { get; set; }

        public Guid EventId { get; set; }


        // Foreign keys
        public virtual Tag Tag { get; set; }

        public virtual Event Event { get; set; }
    }
}