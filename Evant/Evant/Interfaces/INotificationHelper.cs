using System;
using System.Threading.Tasks;

namespace Evant.Interfaces
{
    public interface INotificationHelper
    {
        Task SendFollowNotification(Guid senderId, Guid receiverId);
    }
}
