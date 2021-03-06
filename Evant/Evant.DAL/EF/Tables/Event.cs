﻿using System;
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
            Event_Notifications = new List<Notification>();
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


        public virtual User User { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> EventComments { get; set; }

        public virtual ICollection<EventOperation> EventOperations { get; set; }

        public virtual ICollection<Notification> Event_Notifications { get; set; }
    }
}
