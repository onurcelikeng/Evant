
using Evant.Contracts.DataTransferObjects.UserDevice;
using Evant.NotificationCenter.Models;
using System.Collections.Generic;

namespace Evant.NotificationCenter.Interfaces
{
    public interface IOneSignal
    {
        NotificationResultModel SendNotification(List<string> playerIds, string message);
    }
}
