using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Evant.Storage.Extensions
{
    public static class IFormFileExtensions
    {
        public static string GetFilename(IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
        }

        public static async Task<MemoryStream> GetFileStream(IFormFile file)
        {
            MemoryStream filestream = new MemoryStream();
            await file.CopyToAsync(filestream);
            return filestream;
        }

        public static async Task<byte[]> GetFileArray(IFormFile file)
        {
            MemoryStream filestream = new MemoryStream();
            await file.CopyToAsync(filestream);
            return filestream.ToArray();
        }
    }
}
