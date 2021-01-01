using System.IO;
using System.Threading.Tasks;

namespace Watermark.Services.Storage
{
    public interface IStorage
    {
        Task UploadAsync(Stream stream, string fileName);
        void CreateZip();
        void DeleteUploadFolder();
    }
}
