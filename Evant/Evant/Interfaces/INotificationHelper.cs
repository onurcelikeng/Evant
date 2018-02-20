using System;
using System.Threading.Tasks;

namespace Evant.Interfaces
{
    public interface INotificationHelper
    {
        Task SendFollowNotification(Guid senderId, Guid receiverId);
        Task SendEventAttendNotification(Guid senderId, Guid receiverId);
        Task SendCommentNotification(Guid senderId, Guid receiverId);
        Task SendEventUpdateNotification(Guid receiverId);
    }
}
