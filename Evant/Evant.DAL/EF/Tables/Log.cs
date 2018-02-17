using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Logs")]
    public class Log : BaseEntity
    {
        public string Ip { get; set; }

        public string Table { get; set; }

        public string Message { get; set; } //nullable

        public string Status { get; set; }

        public string Exception { get; set; } //nullable
    }
}
