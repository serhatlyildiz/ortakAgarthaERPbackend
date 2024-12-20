using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Migrations;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductStatusHistoryManager : IProductStatusHistoryService
    {
        IProductStatusHistoryDal _productStatusHistoryDal;

        public ProductStatusHistoryManager(IProductStatusHistoryDal productStatusHistoryDal) 
        {
            _productStatusHistoryDal = productStatusHistoryDal;
        }

        public IDataResult<List<ProductStatusHistory>> GetAll()
        {
            return new SuccessDataResult<List<ProductStatusHistory>>(_productStatusHistoryDal.GetAll(), Messages.ProductsListed);
        }

        public IResult Add(ProductStatusHistory productStatusHistory)
        {
            _productStatusHistoryDal.Add(productStatusHistory);

            return new SuccessResult("Log kaydı Başarılı");
        }

        public IDataResult<List<ProductStatusHistoryDto>> GetProductStatusHistoryWithDetails(int? productStockId = null, DateTime? startDate = null, DateTime? endDate = null, string? productCode = null, string? operations = null)
        {
            using (var context = new NorthwindContext()) // Veritabanı bağlamı
            {
                var query = from history in context.ProductStatusHistories
                            join stock in context.ProductStocks on history.ProductStockId equals stock.ProductStocksId
                            join details in context.ProductDetails on stock.ProductDetailsId equals details.ProductDetailsId
                            join product in context.Products on details.ProductId equals product.ProductId
                            join user in context.Users on history.ChangedBy equals user.Id
                            where (productStockId == null || history.ProductStockId == productStockId)
                               && (startDate == null || history.ChangeDate >= startDate)
                               && (endDate == null || history.ChangeDate <= endDate)
                               && (string.IsNullOrEmpty(productCode) || product.ProductCode.Contains(productCode))
                               && (string.IsNullOrEmpty(operations) || history.Operations.Contains(operations))
                            select new ProductStatusHistoryDto
                            {
                                HistoryId = history.HistoryId,
                                ProductStockId = history.ProductStockId,
                                ProductId = product.ProductId,
                                ProductDetailsId = details.ProductDetailsId,
                                Status = history.Status,
                                ChangedBy = history.ChangedBy,
                                ProductCode = product.ProductCode,
                                ChangedByFirstName = user.FirstName,
                                ChangedByLastName = user.LastName,
                                Email = user.Email,
                                ChangeDate = history.ChangeDate,
                                Operations = history.Operations,
                                Remarks = history.Remarks
                            };

                var result = query.ToList();

                return new SuccessDataResult<List<ProductStatusHistoryDto>>(result, Messages.ProductsListed);
            }
        }
    }
}
