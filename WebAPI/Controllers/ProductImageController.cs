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
            var base64List = new List<string>();
            foreach (var file in files)
            {
                var photoId = Guid.NewGuid(); // GUID ile isimlendirme
                var path = _productImageManager.SavePhoto(photoId, file);
                var base64String = _productImageManager.SavePhoto(photoId, file);

                base64List.Add(base64String);
            }

            return Ok(base64List); // Sadece Base64 string listesini döndür
        }

        [HttpPost("delete")]
        public IActionResult DeletePhoto([FromBody] string fileName)
        {
            if (_productImageManager.DeletePhoto(fileName))
            {
                return Ok(new { Message = "Fotoğraf başarıyla silindi." });
            }

            return NotFound(new { Message = "Fotoğraf bulunamadı." });
        }
    }
}
