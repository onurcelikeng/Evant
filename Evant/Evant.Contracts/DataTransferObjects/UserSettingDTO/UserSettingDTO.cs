using System;

namespace Evant.Contracts.DataTransferObjects.UserSettingDTO
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

        public bool IsCommentVisibleTimeline { get; set; }

        public bool IsFollowerVisibleTimeline { get; set; }

        public bool IsFollowingVisibleTimeline { get; set; }

        public bool IsJoinEventVisibleTimeline { get; set; }
    }
}
