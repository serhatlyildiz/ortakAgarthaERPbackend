using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductStatusHistoryService
    {
        IDataResult<List<ProductStatusHistory>> GetAll();
        IResult Add(ProductStatusHistory productStatusHistory);

        IDataResult<List<ProductStatusHistoryDto>> GetProductStatusHistoryWithDetails(
            int? productStockId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? productCode = null,
            string? operations = null);
        //IDataResult<List<ProductStatusHistory>> GetAllByDate(DateTime firstInterval, DateTime lastInterval);
        //IDataResult<List<ProductStatusHistory>> GetAllByChangedBy(int userId);
        //IDataResult<List<ProductStatusHistory>> GetAllByProductStockId(int productStockId);
    }
}
