using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProductStocksService
    {
        IResult Delete(int productStockId);
        IResult Add(ProductStocks productStocks);
        IDataResult<List<ProductStocks>> GetAll();
        IDataResult<ProductStocks> GetById(int id);
    }
}
