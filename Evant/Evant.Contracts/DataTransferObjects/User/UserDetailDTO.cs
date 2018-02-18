using System;

namespace Evant.Contracts.DataTransferObjects.User
{
    public sealed class UserDetailDTO
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingsCount { get; set; }
    }
}
