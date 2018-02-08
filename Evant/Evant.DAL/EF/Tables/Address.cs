using System;

namespace Evant.DAL.EF.Tables
{
    public class Address : BaseEntity
    {
        public Guid EventId { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }


        // Foreign keys
        public virtual Event Event { get; set; }
    }
}
