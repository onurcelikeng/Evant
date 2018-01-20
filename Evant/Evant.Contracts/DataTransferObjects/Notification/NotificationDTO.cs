using System;
using System.Collections.Generic;
using System.Text;

namespace Evant.Contracts.DataTransferObjects.Notification
{
    public class NotificationDTO : BaseDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string NotificationType { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
