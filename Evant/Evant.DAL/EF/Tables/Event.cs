using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Events")]
    public class Event : BaseEntity
    {
        public Event()
        {
            EventComments = new List<Comment>();
            EventOperations = new List<EventOperation>();
        }


        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }

        [Required, MaxLength(40)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        public string Photo { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [DefaultValue(0)]
        public int TotalParticipants { get; set; }

        [Required]
        [DefaultValue(0)]
        public int TotalComments { get; set; }


        // Foreign keys
        public virtual User User { get; set; }

        public virtual Category Category { get; set; }


        // Reverse navigation
        public virtual ICollection<Comment> EventComments { get; set; }

        public virtual ICollection<EventOperation> EventOperations { get; set; }

    }
}
