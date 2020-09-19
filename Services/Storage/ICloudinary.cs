using CloudinaryDotNet.Actions;
using System.IO;
using System.Threading.Tasks;

namespace Watermark.Services
{
    public interface ICloudinary
    {
        Task<ImageUploadResult> UploadAsync(Stream stream);
    }
}
