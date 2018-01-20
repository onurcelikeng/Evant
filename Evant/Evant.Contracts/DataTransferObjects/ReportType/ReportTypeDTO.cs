using System;

namespace Evant.Contracts.DataTransferObjects.ReportType
{
    public sealed class ReportTypeDTO
    {   
        public Guid ReportTypeId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
    }
}
