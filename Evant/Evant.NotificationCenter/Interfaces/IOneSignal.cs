using System;
using System.Collections.Generic;
using System.Text;

namespace Evant.NotificationCenter.Interfaces
{
    public interface IOneSignal
    {
        void SaveDevice();
        void SendNotification();
    }
}
