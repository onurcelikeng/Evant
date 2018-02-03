using System;

namespace Evant.Contracts.DataTransferObjects.Notification
{
    public sealed class NotificationDTO
    {
        public Guid NotificationId { get; set; }

        public int UserId { get; set; }

        public string NotificationType { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
