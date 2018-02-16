using Evant.Contracts.DataTransferObjects.UserDevice;
using Evant.NotificationCenter.Interfaces;
using Evant.NotificationCenter.Settings;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Evant.NotificationCenter
{
    public class OneSignal : IOneSignal
    {
        private readonly OneSignalSetting _settings;
        private string baseUrl = "https://onesignal.com/api/v1";


        public OneSignal(OneSignalSetting settings)
        {
            _settings = settings;
        }


        public void AddDevice(UserDeviceDTO device)
        {
            var request = WebRequest.Create(baseUrl + "/players") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", "Basic " + _settings.RestApiKey);

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"" + _settings.AppId + "\","
                                                     + "\"identifier\": \"" + device.DeviceId + "\","
                                                      + "\"device_type\": \"" + 0 + "\"}");
            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }
            System.Diagnostics.Debug.WriteLine(responseContent);
        }

        public void SendNotification()
        {

        }

    }
}
