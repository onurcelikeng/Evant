using System;

namespace Evant.Contracts.DataTransferObjects.Event
{
    public sealed class EventShortDTO
    {
        public Guid EventId { get; set; }

        public string PhotoUrl { get; set; }
    }
}
