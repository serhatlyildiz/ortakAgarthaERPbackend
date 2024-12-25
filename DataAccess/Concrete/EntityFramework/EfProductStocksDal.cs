using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductStocksDal : EfEntityRepositoryBase<ProductStocks, NorthwindContext>, IProductStocksDal
    {
        public List<ProductDetailDto2> GetAllByProductDetailsIdAndColor(int productDetailsId, int productColorId)
        {
            using (var context = new NorthwindContext())
            {
                var result = from ps in context.ProductStocks
                             join pd in context.ProductDetails on ps.ProductDetailsId equals pd.ProductDetailsId
                             join p in context.Products on pd.ProductId equals p.ProductId
                             join c in context.Colors on ps.ProductColorId equals c.ColorId
                             join cat in context.Categories on p.CategoryId equals cat.CategoryId
                             join sc in context.SuperCategories on cat.SuperCategoryId equals sc.SuperCategoryId
                             where ps.ProductDetailsId == productDetailsId && ps.ProductColorId == productColorId
                             select new ProductDetailDto2
                             {
                                 ProductDetailsId = ps.ProductDetailsId,
                                 ProductId = p.ProductId,
                                 ColorId = c.ColorId,
                                 ProductStocksId = ps.ProductStocksId,
                                 SuperCategoryId = sc.SuperCategoryId,
                                 CategoryId = cat.CategoryId,
                                 ProductName = p.ProductName,
                                 SuperCategoryName = sc.SuperCategoryName,
                                 CategoryName = cat.CategoryName,
                                 ProductDescription = p.ProductDescription,
                                 UnitPrice = p.UnitPrice,
                                 UnitsInStock = ps.UnitsInStock,
                                 ColorName = c.ColorName,
                                 ProductSize = pd.ProductSize,
                                 Images = ps.Images,
                                 Status = ps.Status,
                                 ProductCode = p.ProductCode,
                                 RowNum = 0 // Gerekirse düzenlenebilir
                             };

                return result.ToList();
            }
        }

        public List<ProductDetailDto2> GetAllByProductDetailsId(int productDetailsId)
        {
            using (var context = new NorthwindContext())
            {
                var result = from ps in context.ProductStocks
                             join pd in context.ProductDetails on ps.ProductDetailsId equals pd.ProductDetailsId
                             join p in context.Products on pd.ProductId equals p.ProductId
                             join cat in context.Categories on p.CategoryId equals cat.CategoryId
                             join sc in context.SuperCategories on cat.SuperCategoryId equals sc.SuperCategoryId
                             where ps.ProductDetailsId == productDetailsId
                             select new ProductDetailDto2
                             {
                                 ProductDetailsId = ps.ProductDetailsId,
                                 ProductId = p.ProductId,
                                 ColorId = ps.ProductColorId,
                                 ProductStocksId = ps.ProductStocksId,
                                 SuperCategoryId = sc.SuperCategoryId,
                                 CategoryId = cat.CategoryId,
                                 ProductName = p.ProductName,
                                 SuperCategoryName = sc.SuperCategoryName,
                                 CategoryName = cat.CategoryName,
                                 ProductDescription = p.ProductDescription,
                                 UnitPrice = p.UnitPrice,
                                 UnitsInStock = ps.UnitsInStock,
                                 ColorName = null, // Color bilgisi eksik olacağından null
                                 ProductSize = pd.ProductSize,
                                 Images = ps.Images,
                                 Status = ps.Status,
                                 ProductCode = p.ProductCode,
                                 RowNum = 0 // Gerekirse düzenlenebilir
                             };

                return result.ToList();
            }
        }
    }
}
