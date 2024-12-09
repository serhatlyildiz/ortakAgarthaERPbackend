using Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly ProductImageManager _productImageManager;

        public ProductImageController(ProductImageManager productImageManager)
        {
            _productImageManager = productImageManager;
        }

        [HttpPost("upload")]
        public IActionResult UploadPhotos([FromForm] List<IFormFile> files)
        {
            var savedPaths = new List<string>();
            foreach (var file in files)
            {
                var photoId = Guid.NewGuid(); // GUID ile isimlendirme
                var path = _productImageManager.SavePhoto(photoId, file);
                savedPaths.Add(path);
            }

            return Ok(savedPaths);
        }
    }
}
