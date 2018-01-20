using System;
using System.Collections.Generic;

namespace Evant.DAL.EF.Tables
{
    public class User : BaseEntity
    {
        public User()
        {
            Followings = new List<FriendOperation>();
            Followers = new List<FriendOperation>();
            Events = new List<Event>();
            EventComments = new List<Comment>();
            EventOperations = new List<EventOperation>();
            Notifications = new List<Notification>();
            UserDevices = new List<UserDevice>();
            UserSearchHistories = new List<SearchHistory>();
            ReportedUsers = new List<UserReport>();
            ReporterUsers = new List<UserReport>();
        }


        public string FacebookId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Photo { get; set; }

        public string UserType { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }


        // Reverse navigation
        public virtual ICollection<FriendOperation> Followings { get; set; }

        public virtual ICollection<FriendOperation> Followers { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Comment> EventComments { get; set; }

        public virtual ICollection<EventOperation> EventOperations { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<UserDevice> UserDevices { get; set; }

        public virtual ICollection<SearchHistory> UserSearchHistories { get; set; }

        public virtual ICollection<UserReport> ReporterUsers { get; set; }

        public virtual ICollection<UserReport> ReportedUsers { get; set; }
    }
}