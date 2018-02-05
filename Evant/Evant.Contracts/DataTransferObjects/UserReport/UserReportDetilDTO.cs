using Evant.Contracts.DataTransferObjects.ReportType;
using Evant.Contracts.DataTransferObjects.User;
using System;

namespace Evant.Contracts.DataTransferObjects.UserReport
{
    public sealed class UserReportDetilDTO
    {
        public Guid UserReportId { get; set; }

        public ReportTypeDTO Report { get; set; }

        public UserInfoDTO ReportedUser { get; set; }

        public UserInfoDTO ReporterUser { get; set; }
    }
}
