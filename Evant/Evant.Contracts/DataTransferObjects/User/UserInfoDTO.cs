using System;

namespace Evant.Contracts.DataTransferObjects.User
{
    public sealed class UserInfoDTO
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }
    }
}
