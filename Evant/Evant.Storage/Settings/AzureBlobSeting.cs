namespace Evant.Storage.Settings
{
    public sealed class AzureBlobSeting
    {
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string EventContainer { get; }
        public string UserContainer { get; }


        public AzureBlobSeting(string storageAccount,
            string storageKey,
            string eventContainer,
            string userContainer)
        {
            this.StorageAccount = storageAccount;
            this.StorageKey = storageKey;
            this.EventContainer = eventContainer;
            this.UserContainer = userContainer;
        }

    }
}
