using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Watermark.Models;

namespace Watermark.Services.Storage
{
    public interface IStorage
    {
        Task UploadFileAsync(List<IFormFile> files);
        Task AddWatermarkAsync(string text, TextWatermarkOptions options);
        void CreateZip();
    }
}
