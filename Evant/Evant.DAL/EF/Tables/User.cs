using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Users")]
    public class User : BaseEntity
    {
        public User()
        {
            Followers = new List<FriendOperation>();
            Followings = new List<FriendOperation>();
            Events = new List<Event>();
            EventComments = new List<Comment>();
            EventOperations = new List<EventOperation>();
            UserDevices = new List<UserDevice>();
            UserSearchHistories = new List<SearchHistory>();
        }


        public string FacebookId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Photo { get; set; } = null;

        [Required]
        public string Role { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool IsFacebook { get; set; }


        public virtual UserSetting Setting { get; set; }

        public virtual ICollection<FriendOperation> Followers { get; set; }

        public virtual ICollection<FriendOperation> Followings { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Comment> EventComments { get; set; }

        public virtual ICollection<EventOperation> EventOperations { get; set; }

        public virtual ICollection<UserDevice> UserDevices { get; set; }

        public virtual ICollection<SearchHistory> UserSearchHistories { get; set; }
    }
}
