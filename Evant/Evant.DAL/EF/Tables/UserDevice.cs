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
        public string DeviceId { get; set; } //retwfdwdr676

        [Required]
        public string Brand { get; set; } //Apple

        [Required]
        public string Model { get; set; } //iPhone 6S

        [Required]
        public string OS { get; set; } //iOS 11

        [Required]
        public bool IsLoggedin { get; set; } = true;


        public virtual User User { get; set; }
    }
}
