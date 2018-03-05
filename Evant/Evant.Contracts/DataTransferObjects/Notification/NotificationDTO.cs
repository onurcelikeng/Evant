using Evant.Contracts.DataTransferObjects.Event;
using Evant.Contracts.DataTransferObjects.User;
using System;

namespace Evant.Contracts.DataTransferObjects.Notification
{
    public sealed class NotificationDTO
    {
        public Guid NotificationId { get; set; }

        public int NotificationType { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserInfoDTO User { get; set; }

        public EventShortDTO Event { get; set; }
    }
}
