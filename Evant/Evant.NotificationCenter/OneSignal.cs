using Evant.Contracts.DataTransferObjects.UserDevice;
using Evant.NotificationCenter.Enums;
using Evant.NotificationCenter.Interfaces;
using Evant.NotificationCenter.Models;
using Evant.NotificationCenter.Serializers;
using Evant.NotificationCenter.Settings;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Evant.NotificationCenter
{
    public class OneSignal : IOneSignal
    {
        private string baseUrl = "https://onesignal.com/api/v1";
        private readonly OneSignalSetting _settings;
        private readonly RestClient _restClient;


        public OneSignal(OneSignalSetting settings)
        {
            _settings = settings;
            _restClient = new RestClient(baseUrl);
        }


        public DeviceResultModel AddDevice(UserDeviceDTO device)
        {
            RestRequest restRequest = new RestRequest("players", Method.POST);

            restRequest.AddHeader("Authorization", string.Format("Basic {0}", _settings.RestApiKey));
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.JsonSerializer = new NewtonsoftJsonSerializer();
            restRequest.AddBody(new DeviceAddOptions()
            {
                AppId = _settings.AppId,
                Identifier = device.DeviceId,
                DeviceType = DeviceTypeEnum.iOS,
                DeviceModel = device.Model,
                DeviceOS = device.OS
            });

            var restResponse = _restClient.Execute<DeviceResultModel>(restRequest);
            if (restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }

            return restResponse.Data;
        }

        public NotificationResultModel SendNotification(string playerId, string message)
        {
            RestRequest restRequest = new RestRequest("notifications", Method.POST);

            restRequest.AddHeader("Authorization", string.Format("Basic {0}", _settings.RestApiKey));

            restRequest.RequestFormat = DataFormat.Json;
            restRequest.JsonSerializer = new NewtonsoftJsonSerializer();

            var options = new NotificationCreateOptions()
            {
                AppId = _settings.AppId,
                Contents = new Dictionary<string, string>
                {
                    { "tr",  message}
                },
                IncludedSegments = new List<string>
                {
                    playerId
                }
            };

            var restResponse = _restClient.Execute<NotificationResultModel>(restRequest);
            if (restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }

            return restResponse.Data;

        }

    }
}
