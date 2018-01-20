using System;

namespace Evant.DAL.EF.Tables
{
    public class FriendOperation : BaseEntity
    {
        public Guid FollowingId { get; set; }

        public Guid FollowerId { get; set; }


        // Foreign keys
        public virtual User FollowerUser { get; set; }

        public virtual User FollowingUser { get; set; }
    }
}