using Microsoft.AspNetCore.Http;

namespace Evant.Storage.Models
{
    public class FileInputModel
    {
        public string Folder { get; set; }

        public IFormFile File { get; set; }
    }
}
