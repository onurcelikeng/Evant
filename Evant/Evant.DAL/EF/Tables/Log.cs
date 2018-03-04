using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Logs")]
    public class Log : BaseEntity
    {
        public string Ip { get; set; }

        [Required]
        public string Controller { get; set; }

        public string Message { get; set; }

        public string Action { get; set; }

        [Required]
        public int StatusCode { get; set; }

        public string Exception { get; set; }
    }
}
