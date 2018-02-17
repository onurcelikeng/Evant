using RestSharp.Deserializers;

namespace Evant.NotificationCenter.Models
{
    public class NotificationResultModel
    {
        [DeserializeAs(Name = "id")]
        public string Id { get; set; }

        [DeserializeAs(Name = "recipients")]
        public int Recipients { get; set; }
    }
}
