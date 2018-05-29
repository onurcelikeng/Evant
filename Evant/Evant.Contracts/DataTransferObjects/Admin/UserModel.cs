using System;

namespace Evant.Contracts.DataTransferObjects.Admin
{
    public sealed class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string Photo { get; set; }
    }
}
