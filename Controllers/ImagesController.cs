using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task UploadFileAsync(List<IFormFile> files)
        {
            await _localFileStorageService.UploadFileAsync(files);
        }

        [Route("watermark")]
        public async Task ProcessImageAsync([FromForm] string text, [FromForm] TextWatermarkOptions options)
        {
            await _localFileStorageService.AddWatermarkAsync(text, options);
        }
    }
}
