using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Watermark.Models;

namespace Watermark.Services.Storage
{
    public class LocalFileStorageService : IStorage
    {
        private readonly string ResourceFolderName = "Resources";

        private readonly string UploadFolderName = "Upload";

        private readonly string WatermarkFolderName = "Watermark";

        private readonly string ArchiveFileName = "Result.zip";

        private string UploadFolder { get; set; }

        private string WatermarkFolder { get; set; }

        public LocalFileStorageService()
        {
            UploadFolder = Path.Combine(ResourceFolderName, UploadFolderName);
            WatermarkFolder = Path.Combine(ResourceFolderName, WatermarkFolderName);
        }

        public async Task UploadFileAsync(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                using var image = Image.FromStream(file.OpenReadStream());
                await SaveImageAsync(image, UploadFolder, file.FileName);
            }
        }

        public async Task AddWatermarkAsync(string text, TextWatermarkOptions options)
        {
            var files = GetFiles();
            Directory.CreateDirectory(WatermarkFolder);

            foreach (var filePath in files)
            {
                using var file = File.OpenRead(Path.Combine(UploadFolder, filePath));
                using var image = Image.FromStream(file);
                image.AddTextWatermark(text, options);

                await SaveImageAsync(image, WatermarkFolder, filePath);
            }
        }

        public void CreateZip()
        {
            var destinationArchiveFilePath = @$"{ResourceFolderName}\{ArchiveFileName}";
            if (File.Exists(destinationArchiveFilePath))
            {
                File.Delete(destinationArchiveFilePath);
            }
            ZipFile.CreateFromDirectory(WatermarkFolder, destinationArchiveFilePath);
        }

        private IEnumerable<string> GetFiles()
        {
            return Directory.EnumerateFiles(UploadFolder).Select(x => Path.GetFileName(x));
        }

        private async Task SaveImageAsync(Image image, string imagePath, string imageName)
        {
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            var imageLocation = Path.Combine(imagePath, imageName);
            using var fileStream = new FileStream(imageLocation, FileMode.Create);
            await memoryStream.CopyToAsync(fileStream);
        }
    }
}
