using Microsoft.AspNetCore.Http;

namespace Evant.Storage.Models
{
    public class FileInputModel
    {
        public IFormFile File { get; set; }
    }
}
