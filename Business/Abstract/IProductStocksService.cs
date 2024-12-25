using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProductStocksService
    {
        IResult Delete(int productStockId);
        IResult Add(ProductStocks productStocks);
        IDataResult<List<ProductStocks>> GetAll();
        IDataResult<ProductStocks> GetById(int id);
        List<ProductDetailDto2> GetAllByProductDetailsIdAndColor(int productDetailsId, int productColorId);
        List<ProductDetailDto2> GetAllByProductDetailsId(int productDetailsId);
        int GetLastProductStockId();
    }
}
