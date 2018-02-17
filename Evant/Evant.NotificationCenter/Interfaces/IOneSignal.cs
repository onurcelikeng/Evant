
using Evant.Contracts.DataTransferObjects.UserDevice;
using Evant.NotificationCenter.Models;

namespace Evant.NotificationCenter.Interfaces
{
    public interface IOneSignal
    {
        DeviceResultModel AddDevice(UserDeviceDTO device);
        NotificationResultModel SendNotification(string playerId, string message);
    }
}
