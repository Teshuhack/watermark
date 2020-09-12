using CloudinaryDotNet.Actions;
using System.Threading.Tasks;

namespace Watermark.Services
{
    public interface ICloudinary
    {
        Task<ImageUploadResult> UploadAsync(string fullImagePath);
    }
}
