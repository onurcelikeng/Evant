
using Evant.Contracts.DataTransferObjects.UserDevice;
using System.Threading.Tasks;

namespace Evant.NotificationCenter.Interfaces
{
    public interface IOneSignal
    {
        void AddDevice(UserDeviceDTO device);
        void SendNotification();
    }
}
