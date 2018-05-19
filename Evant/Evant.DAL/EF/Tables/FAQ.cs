using System;

namespace Evant.DAL.EF.Tables
{
    public class FAQ : BaseEntity
    {
        public Guid EventId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
