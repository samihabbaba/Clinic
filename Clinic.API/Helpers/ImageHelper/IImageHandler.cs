using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Helpers
{
    public interface IImageHandler
    {
        Task<string> UploadImage(IFormFile file);
    }
}
