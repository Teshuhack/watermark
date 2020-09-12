using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Watermark.Services;

namespace Watermark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;
        private ICloudinary _cloudinaryService;

        public ImagesController(IWebHostEnvironment webHostEnvironment, ICloudinary cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("upload")]
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return null;
                }

                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
                string watermarkedImagePath = Path.Combine(path, file.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                using var watermarkedStream = new MemoryStream();
                using var img = Image.FromStream(stream);
                using var graphic = Graphics.FromImage(img);

                var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var point = new Point(img.Width - 120, img.Height - 30);

                graphic.DrawString("test", font, brush, point);

                img.Save(watermarkedStream, ImageFormat.Png);
                img.Save(watermarkedImagePath);

                var response = await _cloudinaryService.UploadAsync(watermarkedImagePath);
                var watermarkedImageUrl = response.SecureUrl.ToString();
                return JsonConvert.SerializeObject(new { watermarkedImageUrl });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
