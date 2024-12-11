using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IProductStocksService
    {
        IResult Delete(int productStockId);
    }
}
