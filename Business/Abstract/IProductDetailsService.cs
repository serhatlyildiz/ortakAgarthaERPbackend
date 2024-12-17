using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProductDetailsService
    {
        IDataResult<List<ProductDetails>> GetAll();
        IResult Add(ProductDetails productStocks);
        IResult Update(ProductDetails productStocks);
        IResult Delete(int productStocksId);
        IDataResult<List<ProductDetails>> GetAllByProductId(int ProductId);
        IDataResult<ProductDetails> GetByProductId(int ProductId);
        List<ProductDetails> GetAllByProductIdAndSize(int productId, string productSize);
    }
}