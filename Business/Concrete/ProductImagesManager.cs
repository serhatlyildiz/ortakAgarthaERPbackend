using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductImagesManager : IProductImagesService
    {
        IProductImagesDal _productImagesDal;

        public ProductImagesManager(IProductImagesDal productImagesDal)
        {
            _productImagesDal = productImagesDal;
        }

        public IResult Add(ProductImages productImages)
        {
            _productImagesDal.Add(productImages);
            return new SuccessResult();
        }

        public IResult Delete(ProductImages productImages)
        {
            _productImagesDal.Delete(productImages);
            return new SuccessResult();
        }

        public IDataResult<List<ProductImages>> GetByProductId(int productId)
        {
            return new SuccessDataResult<List<ProductImages>>(_productImagesDal.GetAll(p => p.ProductId == productId));
        }

        public IResult Update(ProductImages productImages)
        {
            _productImagesDal.Update(productImages);
            return new SuccessResult();
        }
    }
}
