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

namespace Business.Concrete
{
    public class ProductDetailsManager : IProductDetailsService
    {
        IProductDetailsDal _productDetailsDal;

        public ProductDetailsManager(IProductDetailsDal productStocksDal)
        {
            _productDetailsDal = productStocksDal;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductDetailsService.Get")]
        public IResult Add(ProductDetails productStocks)
        {
            _productDetailsDal.Add(productStocks);

            return new SuccessResult(Messages.ProductDetailsAdded);
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductDetailsService.Get")]
        public IResult Delete(int productDetailsId)
        {
            var result = _productDetailsDal.Get(p => p.ProductDetailsId == productDetailsId);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductDetailsNotFound); // Eğer ürün yoksa hata döndür
            }

            // Ürünü sil
            _productDetailsDal.Delete(result);
            return new SuccessResult(Messages.ProductDetailsDeleted);
        }

        [CacheAspect] //key,value
        [PerformanceAspect(5000)]
        public IDataResult<List<ProductDetails>> GetAll()
        {
            return new SuccessDataResult<List<ProductDetails>>(_productDetailsDal.GetAll(), Messages.ProductsListed);
        }

        [CacheAspect]
        public IDataResult<List<ProductDetails>> GetAllByProductId(int ProductId)
        {
            return new SuccessDataResult<List<ProductDetails>>(_productDetailsDal.GetAll(p => p.ProductId == ProductId));
        }

        public IDataResult<ProductDetails> GetById(int productDetailsId)
        {
            return new SuccessDataResult<ProductDetails>(_productDetailsDal.Get(ps => ps.ProductDetailsId == productDetailsId));
        }

        [CacheAspect]
        public IDataResult<ProductDetails> GetByProductId(int ProductId)
        {
            return new SuccessDataResult<ProductDetails>(_productDetailsDal.Get(p => p.ProductId == ProductId));
        }


        [CacheRemoveAspect("IProductStocksService.Get")]
        public IResult Update(ProductDetails productStocks)
        {
            var productStocksToUpdate = _productDetailsDal.Get(p => p.ProductDetailsId == productStocks.ProductDetailsId);

            if (productStocksToUpdate == null)
            {
                return new ErrorResult(Messages.ProductDetailsNotFound); // Eğer ürün yoksa hata döndür
            }

            // Ürünün güncellenmesini sağla
            _productDetailsDal.Update(productStocks);
            return new SuccessResult(Messages.ProductDetailsUpdated); // Başarı mesajı döndür
        }
    }
}