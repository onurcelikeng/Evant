using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("EventOperations")]
    public class EventOperation : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid EventId { get; set; }


        public virtual User User { get; set; }

        public virtual Event Event { get; set; }
    }
}
