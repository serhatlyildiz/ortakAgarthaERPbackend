using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductImageService
    {
        string SavePhoto(Guid photoId, IFormFile file);
        bool DeletePhoto(string fileName);
    }
}
