using Core.DataAccess;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductDal:IEntityRepository<Product>
    {
        List<ProductDetailDto> GetProductDetails();
        List<ProductDetailDto2> GetProductDetails2();
        List<ProductDetailDto> GetProductDetailsWithFilters(ProductFilterModel filter);
        ProductDetails GetProductDetailsById(int productDetailsId);
        ProductStocks GetProductStockById(int productStocksId);

        void Update(Product product);
        void UpdateProductDetails(ProductDetails productDetails);  // Yeni metod
        void UpdateProductStocks(ProductStocks productStocks);
        List<ProductDto> GetProductDto();
        List<ProductDto> GetByProductCodeForProductDto(string productCode);
        List<ProductWithTotalStockDto> GetProductsWithTotalStock();
        List<ProductDetailDto2> GetByCategoryIdProductDetails2(int superCategoryId, int? categoryId);
    }
}
