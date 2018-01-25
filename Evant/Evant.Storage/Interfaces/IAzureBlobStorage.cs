using System.IO;
using System.Threading.Tasks;

namespace Evant.Storage.Interfaces
{
    public interface IAzureBlobStorage
    {
        Task<bool> UploadAsync(string blobName, Stream stream);
    }
}
