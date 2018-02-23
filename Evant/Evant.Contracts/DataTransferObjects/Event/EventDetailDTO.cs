using System;
using System.Collections.Generic;
using Evant.Contracts.DataTransferObjects.Tag;
using Evant.Contracts.DataTransferObjects.User;
using Evant.Contracts.DataTransferObjects.Address;
using Evant.Contracts.DataTransferObjects.Category;

namespace Evant.Contracts.DataTransferObjects.Event
{
    public class EventDetailDTO
    {
        public Guid EventId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }

        public string PhotoUrl { get; set; }

        public int TotalGoings { get; set; }

        public int TotalComments { get; set; }

        public CategoryInfoDTO Category { get; set; }

        public UserInfoDTO User { get; set; }

        public AddressInfoDTO Address { get; set; }

        public List<TagInfoDTO> Tags { get; set; }
    }
}
