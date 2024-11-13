using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductStocksService
    {
        IDataResult<List<ProductStocks>> GetAll();

        IResult Add(ProductStocks productStocks);
        IResult Update(ProductStocks productStocks);
        IResult Delete(int productStocksId);
        IDataResult<List<ProductStocks>> GetAllByProductId(int ProductId);
        IDataResult<ProductStocks> GetByProductId(int ProductId);
    }
}
