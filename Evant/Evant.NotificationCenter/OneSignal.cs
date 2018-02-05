using Evant.NotificationCenter.Interfaces;
using Evant.NotificationCenter.Settings;

namespace Evant.NotificationCenter
{
    public class OneSignal : IOneSignal
    {
        private readonly OneSignalSetting _settings;


        public OneSignal(OneSignalSetting settings)
        {
            _settings = settings;
        }


        public void SaveDevice()
        {

        }

        public void SendNotification()
        {

        }

    }
}
