using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductImagesService
    {
        IDataResult<List<ProductImages>> GetByProductId(int productId);
        IResult Add(ProductImages productImages);
        IResult Update(ProductImages productImages);
        IResult Delete(ProductImages productImages);
    }
}
