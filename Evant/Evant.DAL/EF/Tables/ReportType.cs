using System.Collections;
using System.Collections.Generic;

namespace Evant.DAL.EF.Tables
{
    public class ReportType : BaseEntity
    {
        public ReportType()
        {
            UserReports = new List<UserReport>();
        }


        public string Name { get; set; }

        public int Level { get; set; }


        // Reverse navigation
        public virtual ICollection<UserReport> UserReports { get; set; }
    }
}