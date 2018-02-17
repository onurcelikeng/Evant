using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("UserDevices")]
    public class UserDevice : BaseEntity
    {
        public Guid UserId { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string PlayerId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string OS { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsLoggedin { get; set; }


        // Foreign keys
        public virtual User User { get; set; }
    }
}
