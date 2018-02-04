using System;
using System.Collections.Generic;

namespace Evant.DAL.EF.Tables
{
    public class Event : BaseEntity
    {
        public Event()
        {
            EventTags = new List<EventTag>();
            EventComments = new List<Comment>();
            EventOperations = new List<EventOperation>();
        }


        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid AddressId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public bool IsPrivate { get; set; }

        public string Photo { get; set; }

        public int TotalParticipants { get; set; } = 0;

        public int TotalComments { get; set; } = 0;


        // Foreign keys
        public virtual User User { get; set; }

        public virtual Address EventAddress { get; set; }

        public virtual Category Category { get; set; }


        // Reverse navigation
        public virtual ICollection<EventTag> EventTags { get; set; }

        public virtual ICollection<Comment> EventComments { get; set; }

        public virtual ICollection<EventOperation> EventOperations { get; set; }
    }
}