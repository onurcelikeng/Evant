using Evant.Storage.Interfaces;
using Evant.Storage.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Evant.Storage
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        private readonly AzureBlobSetings _settings;


        public AzureBlobStorage(AzureBlobSetings settings)
        {
            _settings = settings;
        }


        public async Task<bool> UploadAsync(string blobName, Stream stream)
        {
            try
            {
                //Blob
                CloudBlockBlob blockBlob = await GetBlockBlobAsync(blobName);

                //Upload
                stream.Position = 0;
                await blockBlob.UploadFromStreamAsync(stream);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<CloudBlockBlob> GetBlockBlobAsync(string blobName)
        {
            //Container
            CloudBlobContainer blobContainer = await GetContainerAsync();

            //Blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(blobName);

            return blockBlob;
        }

        private async Task<CloudBlobContainer> GetContainerAsync()
        {
            //Account
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(
                    _settings.StorageAccount, 
                    _settings.StorageKey), 
                    false
                );

            //Client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(_settings.UserContainer);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }
    }
}
