using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Watermark.Models;
using Watermark.Services.Storage;

namespace Watermark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IStorage _localFileStorageService;

        public ImagesController(IStorage localFileStorageService)
        {
            _localFileStorageService = localFileStorageService;
        }

        [Route("upload")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public async Task<string> UploadImageAsync(List<IFormFile> images, [FromForm] string watermarkText, [FromForm] TextWatermarkOptions options)
        {
            var responsesUrls = new List<string>();
            foreach (var image in images)
            {
                using var watermarkedStream = new MemoryStream();
                using var img = Image.FromStream(image.OpenReadStream());

                img.AddTextWatermark(watermarkText, options);
                img.Save(watermarkedStream, ImageFormat.Png);
                watermarkedStream.Position = 0;

                await _localFileStorageService.UploadAsync(watermarkedStream, image.FileName);

                responsesUrls.Add(image.FileName);
            }

            _localFileStorageService.CreateZip();
            _localFileStorageService.DeleteUploadFolder();

            return JsonConvert.SerializeObject(new { responsesUrls });
        }
    }
}
