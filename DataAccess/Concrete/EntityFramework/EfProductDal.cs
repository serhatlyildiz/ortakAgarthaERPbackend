using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
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
                                      join pd in context.ProductDetails on p.ProductId equals pd.ProductId
                                      join ps in context.ProductStocks on pd.ProductDetailsId equals ps.ProductDetailsId
                                      join co in context.Colors on ps.ProductColorId equals co.ColorId
                                      select new ProductDetailDto
                                      {
                                          ProductDetailsId = pd.ProductDetailsId,
                                          ProductId = p.ProductId,
                                          ColorId = ps.ProductColorId,
                                          ProductStocksId = ps.ProductStocksId,
                                          SuperCategoryId = c.SuperCategoryId,
                                          CategoryId = c.CategoryId,
                                          ProductName = p.ProductName,
                                          SuperCategoryName = sc.SuperCategoryName,
                                          CategoryName = c.CategoryName,
                                          ProductDescription = p.ProductDescription,
                                          UnitPrice = p.UnitPrice,
                                          UnitsInStock = ps.UnitsInStock,
                                          ColorName = co.ColorName,
                                          ProductSize = pd.ProductSize,
                                          Images = ps.Images ?? new List<string>(), // Eğer resim yoksa boş liste döndür
                                          Status = ps.Status,
                                          ProductCode = p.ProductCode,
                                      }).ToList();

                return productDetails.Where(x => x.Status).ToList();
            }
        }        
        
        
        public List<ProductDetailDto2> GetProductDetails2()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                // Veritabanından temel veriler çekiliyor
                var productDetails = (from p in context.Products
                                      join c in context.Categories on p.CategoryId equals c.CategoryId
                                      join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                                      join pd in context.ProductDetails on p.ProductId equals pd.ProductId
                                      join ps in context.ProductStocks on pd.ProductDetailsId equals ps.ProductDetailsId
                                      join co in context.Colors on ps.ProductColorId equals co.ColorId
                                      where ps.Status == true // Sadece aktif stoklar
                                      select new
                                      {
                                          ProductDetailsId = pd.ProductDetailsId,
                                          ProductId = p.ProductId,
                                          ColorId = ps.ProductColorId,
                                          ProductStocksId = ps.ProductStocksId,
                                          SuperCategoryId = c.SuperCategoryId,
                                          CategoryId = c.CategoryId,
                                          ProductName = p.ProductName,
                                          SuperCategoryName = sc.SuperCategoryName,
                                          CategoryName = c.CategoryName,
                                          ProductDescription = p.ProductDescription,
                                          UnitPrice = p.UnitPrice,
                                          UnitsInStock = ps.UnitsInStock,
                                          ColorName = co.ColorName,
                                          ProductSize = pd.ProductSize,
                                          Images = ps.Images,
                                          ProductCode = p.ProductCode
                                      }).AsEnumerable() // Belleğe alınıyor
                                      .GroupBy(x => x.ProductId) // ProductId ile gruplama
                                      .SelectMany(group => group
                                          .OrderBy(item => item.ProductSize) // Bedene göre sıralama
                                          .Select((item, index) => new ProductDetailDto2
                                          {
                                              ProductDetailsId = item.ProductDetailsId,
                                              ProductId = item.ProductId,
                                              ColorId = item.ColorId,
                                              ProductStocksId = item.ProductStocksId,
                                              SuperCategoryId = item.SuperCategoryId,
                                              CategoryId = item.CategoryId,
                                              ProductName = item.ProductName,
                                              SuperCategoryName = item.SuperCategoryName,
                                              CategoryName = item.CategoryName,
                                              ProductDescription = item.ProductDescription,
                                              UnitPrice = item.UnitPrice,
                                              UnitsInStock = item.UnitsInStock,
                                              ColorName = item.ColorName,
                                              ProductSize = item.ProductSize,
                                              Images = item.Images ?? new List<string>(),
                                              Status = true, // Zaten aktif stokları seçiyoruz
                                              ProductCode = item.ProductCode,
                                              RowNum  = index + 1 // Sıra numarası
                                          }))
                                      .ToList();

                // Sadece RowNum = 1 olanları döndürüyoruz
                return productDetails.Where(x => x.RowNum == 1).ToList();
            }
        }

        public List<ProductDetailDto2> GetByProductIdForProductDetails2(int productId)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                // Veritabanından temel veriler çekiliyor
                var productDetails = (from p in context.Products
                                      join c in context.Categories on p.CategoryId equals c.CategoryId
                                      join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                                      join pd in context.ProductDetails on p.ProductId equals pd.ProductId
                                      join ps in context.ProductStocks on pd.ProductDetailsId equals ps.ProductDetailsId
                                      join co in context.Colors on ps.ProductColorId equals co.ColorId
                                      where ps.Status == true && p.ProductId == productId // Sadece aktif stoklar
                                      select new
                                      {
                                          ProductDetailsId = pd.ProductDetailsId,
                                          ProductId = p.ProductId,
                                          ColorId = ps.ProductColorId,
                                          ProductStocksId = ps.ProductStocksId,
                                          SuperCategoryId = c.SuperCategoryId,
                                          CategoryId = c.CategoryId,
                                          ProductName = p.ProductName,
                                          SuperCategoryName = sc.SuperCategoryName,
                                          CategoryName = c.CategoryName,
                                          ProductDescription = p.ProductDescription,
                                          UnitPrice = p.UnitPrice,
                                          UnitsInStock = ps.UnitsInStock,
                                          ColorName = co.ColorName,
                                          ProductSize = pd.ProductSize,
                                          Images = ps.Images,
                                          ProductCode = p.ProductCode
                                      }).AsEnumerable() // Belleğe alınıyor
                                      .GroupBy(x => x.ProductId) // ProductId ile gruplama
                                      .SelectMany(group => group
                                          .OrderBy(item => item.ProductSize) // Bedene göre sıralama
                                          .Select((item, index) => new ProductDetailDto2
                                          {
                                              ProductDetailsId = item.ProductDetailsId,
                                              ProductId = item.ProductId,
                                              ColorId = item.ColorId,
                                              ProductStocksId = item.ProductStocksId,
                                              SuperCategoryId = item.SuperCategoryId,
                                              CategoryId = item.CategoryId,
                                              ProductName = item.ProductName,
                                              SuperCategoryName = item.SuperCategoryName,
                                              CategoryName = item.CategoryName,
                                              ProductDescription = item.ProductDescription,
                                              UnitPrice = item.UnitPrice,
                                              UnitsInStock = item.UnitsInStock,
                                              ColorName = item.ColorName,
                                              ProductSize = item.ProductSize,
                                              Images = item.Images ?? new List<string>(),
                                              Status = true, // Zaten aktif stokları seçiyoruz
                                              ProductCode = item.ProductCode,
                                              RowNum = index + 1 // Sıra numarası
                                          }))
                                      .ToList();

                // Sadece RowNum = 1 olanları döndürüyoruz
                return productDetails.Where(x => x.RowNum == 1).ToList();
            }
        }

        public List<ProductDetailDto2> GetByCategoryIdProductDetails2(int superCategoryId, int? categoryId)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                // Veritabanı sorgusunu temel al
                var query = from p in context.Products
                            join c in context.Categories on p.CategoryId equals c.CategoryId
                            join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                            join pd in context.ProductDetails on p.ProductId equals pd.ProductId
                            join ps in context.ProductStocks on pd.ProductDetailsId equals ps.ProductDetailsId
                            join co in context.Colors on ps.ProductColorId equals co.ColorId
                            where ps.Status == true // Sadece aktif stoklar
                                  && c.SuperCategoryId == superCategoryId // SuperCategoryId filtresi
                            select new
                            {
                                ProductDetailsId = pd.ProductDetailsId,
                                ProductId = p.ProductId,
                                ColorId = ps.ProductColorId,
                                ProductStocksId = ps.ProductStocksId,
                                SuperCategoryId = c.SuperCategoryId,
                                CategoryId = c.CategoryId,
                                ProductName = p.ProductName,
                                SuperCategoryName = sc.SuperCategoryName,
                                CategoryName = c.CategoryName,
                                ProductDescription = p.ProductDescription,
                                UnitPrice = p.UnitPrice,
                                UnitsInStock = ps.UnitsInStock,
                                ColorName = co.ColorName,
                                ProductSize = pd.ProductSize,
                                Images = ps.Images,
                                ProductCode = p.ProductCode
                            };

                // Eğer categoryId 0 veya null değilse, ek filtre uygula
                if (categoryId.HasValue && categoryId > 0)
                {
                    query = query.Where(x => x.CategoryId == categoryId.Value);
                }

                // Veriyi işleyip DTO'ya dönüştür
                var productDetails = query.AsEnumerable()
                                          .GroupBy(x => x.ProductId) // ProductId ile gruplama
                                          .SelectMany(group => group
                                              .OrderBy(item => item.ProductSize) // Bedene göre sıralama
                                              .Select((item, index) => new ProductDetailDto2
                                              {
                                                  ProductDetailsId = item.ProductDetailsId,
                                                  ProductId = item.ProductId,
                                                  ColorId = item.ColorId,
                                                  ProductStocksId = item.ProductStocksId,
                                                  SuperCategoryId = item.SuperCategoryId,
                                                  CategoryId = item.CategoryId,
                                                  ProductName = item.ProductName,
                                                  SuperCategoryName = item.SuperCategoryName,
                                                  CategoryName = item.CategoryName,
                                                  ProductDescription = item.ProductDescription,
                                                  UnitPrice = item.UnitPrice,
                                                  UnitsInStock = item.UnitsInStock,
                                                  ColorName = item.ColorName,
                                                  ProductSize = item.ProductSize,
                                                  Images = item.Images ?? new List<string>(),
                                                  Status = true, // Zaten aktif stokları seçiyoruz
                                                  ProductCode = item.ProductCode,
                                                  RowNum = index + 1 // Sıra numarası
                                              }))
                                          .ToList();

                // Sadece RowNum = 1 olanları döndür
                return productDetails.Where(x => x.RowNum == 1).ToList();
            }
        }


        public List<ProductDto> GetProductDto()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var productDto = (from p in context.Products
                                      join c in context.Categories on p.CategoryId equals c.CategoryId
                                      join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                                      select new ProductDto
                                      {
                                          ProductId = p.ProductId,
                                          SuperCategoryId = c.SuperCategoryId,
                                          CategoryId = c.CategoryId,
                                          ProductName = p.ProductName,
                                          SuperCategoryName = sc.SuperCategoryName,
                                          CategoryName = c.CategoryName,
                                          ProductDescription = p.ProductDescription,
                                          UnitPrice = p.UnitPrice,
                                          Status = p.Status,
                                          ProductCode = p.ProductCode,
                                      }).ToList();

                return productDto.Where(x => x.Status).ToList();
            }
        }

        public List<ProductDto> GetByProductCodeForProductDto(string productCode)
        {
            productCode = productCode.ToUpper();
            using (NorthwindContext context = new NorthwindContext())
            {
                var productDto = (from p in context.Products
                                  join c in context.Categories on p.CategoryId equals c.CategoryId
                                  join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                                  where p.ProductCode == productCode
                                  select new ProductDto
                                  {
                                      ProductId = p.ProductId,
                                      SuperCategoryId = c.SuperCategoryId,
                                      CategoryId = c.CategoryId,
                                      ProductName = p.ProductName,
                                      SuperCategoryName = sc.SuperCategoryName,
                                      CategoryName = c.CategoryName,
                                      ProductDescription = p.ProductDescription,
                                      UnitPrice = p.UnitPrice,
                                      Status = p.Status,
                                      ProductCode = p.ProductCode,
                                  }).ToList();

                return productDto.Where(x => x.Status).ToList();
            }
        }


        public List<ProductDetailDto> GetProductDetailsWithFilters(ProductFilterModel filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var query = from p in context.Products
                            join c in context.Categories on p.CategoryId equals c.CategoryId
                            join sc in context.SuperCategories on c.SuperCategoryId equals sc.SuperCategoryId
                            join pd in context.ProductDetails on p.ProductId equals pd.ProductId
                            join ps in context.ProductStocks on pd.ProductDetailsId equals ps.ProductDetailsId
                            join co in context.Colors on ps.ProductColorId equals co.ColorId
                            select new ProductDetailDto
                            {
                                ProductDetailsId = pd.ProductDetailsId,
                                ProductId = p.ProductId,
                                ColorId = ps.ProductColorId,
                                ProductStocksId = ps.ProductStocksId,
                                SuperCategoryId = c.SuperCategoryId,
                                CategoryId = c.CategoryId,
                                ProductName = p.ProductName,
                                SuperCategoryName = sc.SuperCategoryName,
                                CategoryName = c.CategoryName,
                                ProductDescription = p.ProductDescription,
                                UnitPrice = p.UnitPrice,
                                UnitsInStock = ps.UnitsInStock,
                                ColorName = co.ColorName,
                                ProductSize = pd.ProductSize,
                                Images = ps.Images ?? new List<string>(), // Eğer resim yoksa boş liste döndür
                                Status = ps.Status,
                                ProductCode = p.ProductCode
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

                if (filter.MaxStock.HasValue && filter.MaxStock.Value > 0)
                    query = query.Where(q => q.UnitsInStock <= filter.MaxStock.Value);

                if (!string.IsNullOrEmpty(filter.ColorName))
                    query = query.Where(q => q.ColorName.Contains(filter.ColorName));

                if (!string.IsNullOrEmpty(filter.ProductSize))
                    query = query.Where(q => q.ProductSize.Equals(filter.ProductSize));

                if (filter.Status.HasValue)
                    query = query.Where(q => q.Status == filter.Status.Value);

                if (!string.IsNullOrEmpty(filter.ProductCode))
                    query = query.Where(q => q.ProductCode.Equals(filter.ProductCode));

                return query.ToList();
            }
        }

        public void Update(Product product)
        {
            using (var context = new NorthwindContext())
            {
                context.Products.Update(product);
                context.SaveChanges();
            }
        }

        public void UpdateProductDetails(ProductDetails productDetails)
        {
            using (var context = new NorthwindContext())
            {
                context.ProductDetails.Update(productDetails);
                context.SaveChanges();
            }
        }

        public void UpdateProductStocks(ProductStocks productStocks)
        {
            using (var context = new NorthwindContext())
            {
                context.ProductStocks.Update(productStocks);
                context.SaveChanges();
            }
        }

        public ProductDetails GetProductDetailsById(int productDetailsId)
        {
            using (var context = new NorthwindContext())
            {
                return context.ProductDetails.FirstOrDefault(pd => pd.ProductDetailsId == productDetailsId);
            }
        }

        public ProductStocks GetProductStockById(int productStocksId)
        {
            using (var context = new NorthwindContext())
            {
                return context.ProductStocks.FirstOrDefault(ps => ps.ProductStocksId == productStocksId);
            }
        }

        public List<ProductWithTotalStockDto> GetProductsWithTotalStock()
        {
            using (var context = new NorthwindContext())
            {
                var result = from p in context.Products
                             join pd in context.ProductDetails on p.ProductId equals pd.ProductId
                             join ps in context.ProductStocks on pd.ProductDetailsId equals ps.ProductDetailsId
                             group ps by new { p.ProductId, p.ProductName, p.ProductCode, p.UnitPrice, p.CategoryId } into g
                             select new ProductWithTotalStockDto
                             {
                                 ProductId = g.Key.ProductId,
                                 ProductName = g.Key.ProductName,
                                 ProductCode = g.Key.ProductCode,
                                 UnitPrice = g.Key.UnitPrice,
                                 TotalStock = g.Sum(x => x.UnitsInStock) // Toplam stok hesaplaması
                             };

                return result.ToList();
            }
        }

    }
}
