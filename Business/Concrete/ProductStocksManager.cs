using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
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
    public class ProductStocksManager : IProductStocksService
    {
        IProductStocksDal _productStocksDal;

        public ProductStocksManager(IProductStocksDal productStocksDal)
        {
            _productStocksDal = productStocksDal;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductStocksService.Get")]
        public IResult Add(ProductStocks productStocks)
        {
            _productStocksDal.Add(productStocks);

            return new SuccessResult(Messages.ProductStocksAdded);
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductStocksService.Get")]
        public IResult Delete(int productStocksId)
        {
            var result = _productStocksDal.Get(p => p.ProductStockId == productStocksId);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductStocksNotFound); // Eğer ürün yoksa hata döndür
            }

            // Ürünü sil
            _productStocksDal.Delete(result);
            return new SuccessResult(Messages.ProductStocksDeleted);
        }

        [CacheAspect] //key,value
        [PerformanceAspect(5000)]
        public IDataResult<List<ProductStocks>> GetAll()
        {
            return new SuccessDataResult<List<ProductStocks>>(_productStocksDal.GetAll(), Messages.ProductsListed);
        }
        
        [CacheAspect]
        public IDataResult<List<ProductStocks>> GetAllByProductId(int ProductId)
        {
            return new SuccessDataResult<List<ProductStocks>>(_productStocksDal.GetAll(p => p.ProductId == ProductId));
        }

        [CacheAspect]
        public IDataResult<ProductStocks> GetByProductId(int ProductId)
        {
            return new SuccessDataResult<ProductStocks>(_productStocksDal.Get(p => p.ProductId == ProductId));
        }
        

        [CacheRemoveAspect("IProductStocksService.Get")]
        public IResult Update(ProductStocks productStocks)
        {
            var productStocksToUpdate = _productStocksDal.Get(p => p.ProductStockId == productStocks.ProductStockId);

            if (productStocksToUpdate == null)
            {
                return new ErrorResult(Messages.ProductStocksNotFound); // Eğer ürün yoksa hata döndür
            }

            // Ürünün güncellenmesini sağla
            _productStocksDal.Update(productStocks);
            return new SuccessResult(Messages.ProductStocksUpdated); // Başarı mesajı döndür
        }
    }
}
