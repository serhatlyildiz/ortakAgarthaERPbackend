using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
        private readonly IMapper _mapper;

        public EfProductDal(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<ProductDetailDto> GetProductDetails()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var productDetails = (from p in context.Products
                                      join c in context.Categories on p.CategoryId equals c.CategoryId
                                      join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                                      join ps in context.ProductStocks on p.ProductId equals ps.ProductId
                                      join co in context.Colors on ps.ProductColorId equals co.ColorId
                                      select new ProductDetailDto
                                      {
                                          ProductId = p.ProductId,
                                          CategoryId = c.CategoryId,
                                          SuperCategoryId = c.SuperCategoryId,
                                          ColorId = ps.ProductColorId,
                                          ProductStocksId = ps.ProductStockId,
                                          ProductName = p.ProductName,
                                          ProductDescription = p.ProductDescription,
                                          UnitPrice = p.UnitPrice,
                                          UnitsInStock = ps.UnitsInStock,
                                          ProductSize = ps.ProductSize,
                                          ColorName = co.ColorName,
                                          Images = ps.Images ?? new List<string>(), // Eğer resim yoksa boş liste döndür
                                          CategoryName = c.CategoryName,
                                          SuperCategoryName = sc.SuperCategoryName,
                                          Status = ps.Status
                                      }).ToList();

                return productDetails;
            }
        }

        public List<ProductDetailDto> GetProductDetailsWithFilters(ProductFilterModel filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var query = from p in context.Products
                            join c in context.Categories on p.CategoryId equals c.CategoryId
                            join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                            join ps in context.ProductStocks on p.ProductId equals ps.ProductId
                            join co in context.Colors on ps.ProductColorId equals co.ColorId
                            select new ProductDetailDto
                            {
                                ProductId = p.ProductId,
                                CategoryId = c.CategoryId,
                                SuperCategoryId = c.SuperCategoryId,
                                ColorId = ps.ProductColorId,
                                ProductStocksId = ps.ProductStockId,
                                ProductName = p.ProductName,
                                ProductDescription = p.ProductDescription,
                                UnitPrice = p.UnitPrice,
                                UnitsInStock = ps.UnitsInStock,
                                ProductSize = ps.ProductSize,
                                ColorName = co.ColorName,
                                Images = ps.Images ?? new List<string>(), // Eğer resim yoksa boş liste döndür
                                CategoryName = c.CategoryName,
                                SuperCategoryName = sc.SuperCategoryName,
                                Status = ps.Status
                            };

                // Dinamik filtreleme
                if (!string.IsNullOrEmpty(filter.ProductName))
                    query = query.Where(q => q.ProductName.Contains(filter.ProductName));
                if (!string.IsNullOrEmpty(filter.SuperCategoryName))
                    query = query.Where(q => q.SuperCategoryName.Contains(filter.SuperCategoryName));
                if (!string.IsNullOrEmpty(filter.CategoryName))
                    query = query.Where(q => q.CategoryName.Contains(filter.CategoryName));
                if (filter.MinPrice.HasValue)
                    query = query.Where(q => q.UnitPrice >= filter.MinPrice.Value);
                if (filter.MaxPrice.HasValue)
                    query = query.Where(q => q.UnitPrice <= filter.MaxPrice.Value);
                if (filter.MinStock.HasValue)
                    query = query.Where(q => q.UnitsInStock >= filter.MinStock.Value);
                if (filter.MaxStock.HasValue)
                    query = query.Where(q => q.UnitsInStock <= filter.MaxStock.Value);
                if (!string.IsNullOrEmpty(filter.ColorName))
                    query = query.Where(q => q.ColorName.Contains(filter.ColorName));
                if (!string.IsNullOrEmpty(filter.ProductSize))
                    query = query.Where(q => q.ProductSize.Contains(filter.ProductSize));
                if (filter.Status.HasValue)
                    query = query.Where(q => q.Status == filter.Status.Value);

                return query.ToList();
            }
        }

    }
}
