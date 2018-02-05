using System;

namespace Evant.Contracts.DataTransferObjects.UserSettingDTO.cs
{
    public sealed class UserSettingDTO
    {
        public Guid UserSettingId { get; set; }

        public string Theme { get; set; }

        public string Language { get; set; }

        public bool IsFriendshipNotif { get; set; }

        public bool IsCommentNotif { get; set; }

        public bool IsEventNewComerNotif { get; set; }

        public bool IsEventUpdateNotif { get; set; }
    }
}
