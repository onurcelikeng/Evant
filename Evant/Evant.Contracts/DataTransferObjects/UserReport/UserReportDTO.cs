using System;

namespace Evant.Contracts.DataTransferObjects.UserReport
{
    public sealed class UserReportDTO
    {
        public Guid ReportedUserId { get; set; }

        public Guid ReportTypeId { get; set; }
    }
}
