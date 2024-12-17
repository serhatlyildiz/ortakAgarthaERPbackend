using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ProductStocksManager : IProductStocksService
    {
        IProductStocksDal _productStocksDal;

        public ProductStocksManager(IProductStocksDal productStocksDal)
        {
            _productStocksDal = productStocksDal;
        }

        public IResult Add(ProductStocks productStocks)
        {
            _productStocksDal.Add(productStocks);
            return new SuccessResult();
        }

        public IResult Delete(int productStockId)
        {
            var result = _productStocksDal.Get(x => x.ProductStocksId == productStockId);

            if (result==null)
            {
                return new ErrorResult("Ürün zaten mevcut değil");
            }
            _productStocksDal.Delete(result);
            return new SuccessResult("Ürün başarıyla kaldırıldı");
        }

        public IDataResult<ProductStocks> GetById(int id)
        {
            return new SuccessDataResult<ProductStocks>(_productStocksDal.Get(x => x.ProductStocksId == id));
        }

        public IDataResult<List<ProductStocks>> GetAll()
        {
            return new SuccessDataResult<List<ProductStocks>>(_productStocksDal.GetAll());
        }

        public List<ProductStocks> GetAllByProductDetailsIdAndColor(int productDetailsId, int productColorId)
        {
            return _productStocksDal.GetAll(p => p.ProductDetailsId == productDetailsId && p.ProductColorId == productColorId);
        }
    }
}
