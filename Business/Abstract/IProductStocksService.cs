using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProductStocksService
    {
        IDataResult<List<ProductStocks>> GetAll();
        IResult Add(ProductStocks productStocks);
        IResult Update(ProductStocks productStocks);
        IResult Delete(int productStocksId);
        IDataResult<ProductStocks> GetById(int productStocksId);
        IDataResult<List<ProductStocks>> GetAllByProductId(int ProductId);
        IDataResult<ProductStocks> GetByProductId(int ProductId);
    }
}