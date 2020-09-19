using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Watermark.Models;
using Watermark.Services;

namespace Watermark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ICloudinary _cloudinaryService;

        public ImagesController(ICloudinary cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [Route("upload")]
        public async Task<string> UploadImageAsync([FromForm] UploadInfo uploadInfo)
        {
            try
            {
                var images = uploadInfo.Images;
                var watermarkText = uploadInfo.WatermarkText;

                using var watermarkedStream = new MemoryStream();
                using var img = Image.FromStream(images.OpenReadStream());
                using var graphic = Graphics.FromImage(img);

                var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var point = new Point(img.Width - 120, img.Height - 30);

                graphic.DrawString("test", font, brush, point);

                img.Save(watermarkedStream, ImageFormat.Png);
                watermarkedStream.Position = 0;

                var response = await _cloudinaryService.UploadAsync(watermarkedStream);
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
