using System;

namespace Evant.DAL.EF.Tables
{
    public class UserReport : BaseEntity
    {
        public Guid ReportTypeId { get; set; }

        public Guid ReporterUserId { get; set; }

        public Guid ReportedUserId { get; set; }


        // Foreign keys
        public virtual ReportType ReportType { get; set; }

        public virtual User ReporterUser { get; set; }

        public virtual User ReportedUser { get; set; }
    }
}