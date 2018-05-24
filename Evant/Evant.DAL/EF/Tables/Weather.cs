using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Weather")]
    public class Weather : BaseEntity
    {
        public string City { get; set; }

        public string Town { get; set; }

        public string Status { get; set; }

        public string Content { get; set; }
    }
}
