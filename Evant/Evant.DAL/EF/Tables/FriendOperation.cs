using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("FriendOperations")]
    public class FriendOperation : BaseEntity
    {
        public Guid FollowingUserId { get; set; } //takip edilen

        public Guid FollowerUserId { get; set; } //takip eden


        public virtual User FollowerUser { get; set; }

        public virtual User FollowingUser { get; set; }
    }
}
