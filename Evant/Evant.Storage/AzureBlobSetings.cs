using System;

namespace Evant.Storage
{
    public class AzureBlobSetings
    {
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string EventContainer { get; }
        public string UserContainer { get; }


        public AzureBlobSetings(string storageAccount, string storageKey, string eventContainer, string userContainer)
        {
            if (string.IsNullOrEmpty(storageAccount))
                throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrEmpty(eventContainer))
                throw new ArgumentNullException("EventContainer");

            if (string.IsNullOrEmpty(userContainer))
                throw new ArgumentNullException("UserContainer");

            this.StorageAccount = storageAccount;
            this.StorageKey = storageKey;
            this.EventContainer = eventContainer;
            this.UserContainer = userContainer;
        }

    }
}
