namespace Evant.NotificationCenter.Settings
{
    public sealed class OneSignalSetting
    {
        public string AppId { get; }
        public string RestApiKey { get; }


        public OneSignalSetting(string appId, string restApiKey)
        {
            this.AppId = appId;
            this.RestApiKey = restApiKey;
        }

    }
}
