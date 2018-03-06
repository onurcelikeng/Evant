using Evant.Constants;
using Evant.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Evant.Helpers
{
    public class SlackHelper
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding;


        public SlackHelper()
        {
            _uri = new Uri(SlackConstant.Uri);
            _encoding = new UTF8Encoding();
        }


        public void PostMessage(string text)
        {
            SlackModel model = new SlackModel()
            {
                Channel = SlackConstant.Channel,
                Username = SlackConstant.Username,
                Text = text
            };

            string payloadJson = JsonConvert.SerializeObject(model);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection
                {
                    ["payload"] = payloadJson
                };

                var response = client.UploadValues(_uri, "POST", data);

                string responseText = _encoding.GetString(response);
            }
        }

    }
}
