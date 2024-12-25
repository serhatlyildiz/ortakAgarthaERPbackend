using Core.DataAccess;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductStocksDal : IEntityRepository<ProductStocks>
    {
        List<ProductDetailDto2> GetAllByProductDetailsIdAndColor(int productDetailsId, int productColorId);

        List<ProductDetailDto2> GetAllByProductDetailsId(int productDetailsId);
    }
}
