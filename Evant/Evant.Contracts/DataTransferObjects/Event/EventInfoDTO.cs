using System;

namespace Evant.Contracts.DataTransferObjects.Event
{
    public sealed class EventInfoDTO
    {
        public Guid EventId { get; set; }

        public string Title { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime Start { get; set; }
    }
}
