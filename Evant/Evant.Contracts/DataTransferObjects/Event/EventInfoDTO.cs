using System;
using Evant.Contracts.DataTransferObjects.User;

namespace Evant.Contracts.DataTransferObjects.Event
{
    public sealed class EventInfoDTO
    {
        public Guid EventId { get; set; }

        public string Title { get; set; }

        public string PhotoUrl { get; set; }

        public int TotalGoings { get; set; }

        public int TotalComments { get; set; }

        public DateTime Start { get; set; }

        public UserInfoDTO User { get; set; }
    }
}
