using Microsoft.AspNetCore.Http;

namespace Watermark.Models
{
    public class UploadInfo
    {
        public IFormFile Images { get; set; }

        public string WatermarkText { get; set; }
    }
}
