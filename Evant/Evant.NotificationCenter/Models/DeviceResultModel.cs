﻿using RestSharp.Deserializers;

namespace Evant.NotificationCenter.Models
{
    public class DeviceResultModel
    {
        [DeserializeAs(Name = "success")]
        public bool IsSuccess { get; set; }

        [DeserializeAs(Name = "id")]
        public string Id { get; set; }
    }
}
