using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductStatusHistoryDal : EfEntityRepositoryBase<ProductStatusHistory, NorthwindContext>, IProductStatusHistoryDal
    {
        
    }
}