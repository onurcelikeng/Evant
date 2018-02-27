using System;

namespace Evant.Contracts.DataTransferObjects.Timeline
{
    public sealed class TimelineDTO
    {
        public string Header { get; set; }

        public string Body { get; set; }

        public string Type { get; set; }

        public Guid CustomId { get; set; }

        public string Image { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
