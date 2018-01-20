using System;

namespace Evant.Contracts.DataTransferObjects.Event
{
    public sealed class NewEventDTO
    {
        public Guid CategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool isPrivate { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime FinishAt { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
