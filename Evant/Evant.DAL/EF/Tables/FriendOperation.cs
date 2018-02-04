using System;

namespace Evant.DAL.EF.Tables
{
    public class FriendOperation : BaseEntity
    {
        public Guid FollowingId { get; set; } //takip edilen

        public Guid FollowerId { get; set; } //takip eden


        // Foreign keys
        public virtual User FollowerUser { get; set; }

        public virtual User FollowingUser { get; set; }
    }
}
