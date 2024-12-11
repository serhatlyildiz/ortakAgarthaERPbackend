using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class ProductStocksManager : IProductStocksService
    {
        IProductStocksDal _productStocksDal;

        public ProductStocksManager(IProductStocksDal productStocksDal)
        {
            _productStocksDal = productStocksDal;
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
    }
}
