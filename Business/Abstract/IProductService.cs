using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<ProductDetailDto2>> GetProductDetails2();
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GetById(int productId);
        IResult Add(ProductUpdateDto productUpdateDto);
        IResult Update(Product product, ProductDetails productDetails, ProductStocks productStocks);
        IResult Delete(int productID);
        IDataResult<List<ProductDetailDto>> GetProductDetailsWithFilters(ProductFilterModel filter);
        IResult Restore(int productID);
        IDataResult<List<ProductDetailDto>> GetProductStockDetails(int productStockId);
        IDataResult<List<ProductDto>> GetProductDto();
        IDataResult<List<ProductDto>> GetByProductCodeForProductDto(string productCode);
        IResult StockProductAdd(Product product);
        IDataResult<List<ProductDetailDto>> GetProductStockDetailsByProduct(int productId);
    }
}