using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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

        public async Task<ImageUploadResult> UploadAsync(Stream stream)
        {
            return await Cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(Guid.NewGuid().ToString() , stream)
            });
        }
    }
}
