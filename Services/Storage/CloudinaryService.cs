using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Watermark.Models;

namespace Watermark.Services.Storage
{
    public class CloudinaryService : ICloudinary
    {
        private readonly CloudinarySettings _imageStorage;

        private Cloudinary Cloudinary { get; }

        public CloudinaryService(IOptions<CloudinarySettings> imageStorage)
        {
            _imageStorage = imageStorage.Value;

            Cloudinary = new Cloudinary(_imageStorage.CloudinaryUrl);
        }

        public async Task<ImageUploadResult> UploadAsync(string fullImagePath)
        {
            return await Cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription($"{fullImagePath}")
            });
        }
    }
}
