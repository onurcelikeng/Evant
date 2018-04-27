using System;

namespace Evant.Contracts.DataTransferObjects.Event
{
    public sealed class EventDTO
    {
        public Guid EventId { get; set; }

        public Guid CategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime FinishAt { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
