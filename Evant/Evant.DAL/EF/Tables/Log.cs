namespace Evant.DAL.EF.Tables
{
    public class Log : BaseEntity
    {
        public string Ip { get; set; }

        public string Table { get; set; }

        public string Message { get; set; } //operation

        public string Status { get; set; }

        public string Exception { get; set; }
    }
}
