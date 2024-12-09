using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GetById(int productId);
        IResult Add(Product product);
        IResult Update(Product product, ProductDetails productDetails, ProductStocks productStocks);
        IResult Delete(int productID);
        IDataResult<List<ProductDetailDto>> GetProductDetailsWithFilters(ProductFilterModel filter);
        IResult Restore(int productID);
        IDataResult<List<ProductDetailDto>> GetProductStockDetails(int productStockId);
    }
}