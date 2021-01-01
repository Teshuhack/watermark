using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Watermark.Services.Storage
{
    public class LocalFileStorageService : IStorage
    {
        private readonly string ResourceFolderName = "Resources";

        private readonly string UploadFolderName = "Upload";

        private readonly string ArchiveFileName = "Result.zip";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalFileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task UploadAsync(Stream stream, string fileName)
        {
            var fulllResourcePath = Path.Combine(_webHostEnvironment.ContentRootPath, ResourceFolderName, UploadFolderName);
            Directory.CreateDirectory(fulllResourcePath);

            var fileLocation = Path.Combine(fulllResourcePath, fileName);
            using var fileStream = new FileStream(fileLocation, FileMode.Create);

            await stream.CopyToAsync(fileStream);
        }

        public void CreateZip()
        {
            var sourceDirectoryName = @$"{ResourceFolderName}\{UploadFolderName}";
            var destinationArchiveFilePath = @$"{ResourceFolderName}\{ArchiveFileName}";
            if (File.Exists(destinationArchiveFilePath))
            {
                File.Delete(destinationArchiveFilePath);
            }
            ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFilePath);
        }

        public void DeleteUploadFolder()
        {
            Directory.Delete(@$"{ResourceFolderName}\{UploadFolderName}", true);
        }
    }
}
