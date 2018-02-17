using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Evant.NotificationCenter.Serializers
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        public string ContentType
        {
            get { return "application/json"; }
            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        private JsonSerializerSettings Settings;


        public NewtonsoftJsonSerializer()
        {
            Settings = new JsonSerializerSettings();
            Settings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
