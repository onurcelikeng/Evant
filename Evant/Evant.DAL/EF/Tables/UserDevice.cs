using System;

namespace Evant.DAL.EF.Tables
{
    public class UserDevice : BaseEntity
    {
        public Guid UserId { get; set; }

        public string DeviceId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string OS { get; set; }

        public bool IsLoggedin { get; set; }


        // Foreign keys
        public virtual User User { get; set; }
    }
}