using Business.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ProductImageManager> _logger;
        private readonly string _targetFolder;

        // Yükleme yapılacak hedef klasör
        //private readonly string _targetFolder = @"C:\kamp-frontend\ortakAgarthaERPfrontend\src\assets\productImages";

        public ProductImageManager(IWebHostEnvironment webHostEnvironment, ILogger<ProductImageManager> logger, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _targetFolder = configuration.GetSection("FileSettings:UploadPath").Value;
        }

        public string SavePhoto(Guid photoId, IFormFile file)
        {
            try
            {
                // Fotoğrafların kaydedileceği dizin
                var uploadPath = @"C:\kamp-frontend\ortakAgarthaERPfrontend\src\assets\productImages";

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Dosyanın tam yolunu GUID ile oluştur
                var filePath = Path.Combine(uploadPath, $"{photoId}{Path.GetExtension(file.FileName)}");

                // Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Göreceli yolu frontend'e döndür
                return $"assets/productImages/{photoId}{Path.GetExtension(file.FileName)}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fotoğraf kaydedilirken hata oluştu: {ex.Message}");
                throw new Exception("Fotoğraf yükleme hatası: " + ex.Message);
            }
        }


        public bool DeletePhoto(string fileName)
        {
            try
            {
                var uploadPath = @"C:\kamp-frontend\ortakAgarthaERPfrontend\src\assets\productImages";
                var filePath = Path.Combine(uploadPath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation($"Fotoğraf silindi: {filePath}");
                    return true;
                }

                _logger.LogWarning($"Fotoğraf bulunamadı: {filePath}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fotoğraf silinirken hata oluştu: {ex.Message}");
                throw new Exception("Fotoğraf silme hatası: " + ex.Message);
            }
        }

    }
}
