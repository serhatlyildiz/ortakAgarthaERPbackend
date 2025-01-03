﻿using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System.Transactions;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _ProductDal;
        ICategoryService _categoryService;
        IProductDetailsService _productDetailsService;
        IProductStocksService _productStockService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService, IProductDetailsService productDetailsService, IProductStocksService productStockService)
        {
            _ProductDal = productDal;
            _categoryService = categoryService;
            _productDetailsService = productDetailsService;
            _productStockService = productStockService;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(ProductUpdateDto productUpdateDto)
        {

            _ProductDal.Add(productUpdateDto.Product);

            var resultProduct = _ProductDal.GetAll().Last();
            productUpdateDto.ProductDetails.ProductId = resultProduct.ProductId;

            _productDetailsService.Add(productUpdateDto.ProductDetails);

            var resultProductDetail = _productDetailsService.GetAll().Data.Last();
            productUpdateDto.ProductStocks.ProductDetailsId = resultProductDetail.ProductDetailsId;

            _productStockService.Add(productUpdateDto.ProductStocks);

            return new SuccessResult(Messages.ProductAdded);
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult ProductAdd(Product product)
        {
            // Business rule: Product code must be in uppercase
            product.ProductCode = product.ProductCode.ToUpper();

            // Business rule: Check if the product code is reserved (e.g., "navi")
            if (product.ProductCode.ToLower() == "navi")
            {
                return new ErrorResult("Product code cannot be 'navi'.");
            }


            // Business rules checks
            IResult result = BusinessRules.Run(
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryLimitExceded(),
                CheckIfProductCodeExists(product.ProductCode)  // Check if the product code already exists
            );

            if (result != null) return result;

            _ProductDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);

        }

        [SecuredOperation("product.add,admin")]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult ProductStockAdd(ProductStockAddDto productStockAddDto)
        {
            int productDetailsIdPrivate;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // 1. Aynı ProductId ve ProductSize için tüm ProductDetailsId'leri al
                    var matchingProductDetailsList = _productDetailsService
                        .GetAllByProductIdAndSize(productStockAddDto.ProductId, productStockAddDto.ProductSize);

                    // 2. Bu ProductDetailsId'ler ile ProductStocks tablosunda ColorId kontrolü yap
                    foreach (var productDetails in matchingProductDetailsList)
                    {
                        // Bu ProductDetailsId ile aynı renk olup olmadığını kontrol et
                        var existingProductStocks = _productStockService.GetAllByProductDetailsIdAndColor(
                            productDetails.ProductDetailsId,
                            productStockAddDto.ProductColorId
                        );

                        if (existingProductStocks != null && existingProductStocks.Any())
                        {
                            // Aynı renk için stok bulundu, işlem iptal
                            return new ErrorResult("Bu ürün boyutu ve rengi için stok zaten mevcut.");
                        }
                    }

                    if (matchingProductDetailsList.Count>0)
                    {
                        productDetailsIdPrivate = Convert.ToInt32(matchingProductDetailsList[0].ProductDetailsId);
                    }
                    else
                    {
                        // 3. ProductDetails ekleme (eğer mevcut değilse)
                        var productDetailsEntity = new ProductDetails
                        {
                            ProductId = productStockAddDto.ProductId,
                            ProductSize = productStockAddDto.ProductSize,
                            Status = productStockAddDto.Status,
                            ProductCode = productStockAddDto.ProductCode
                        };

                        _productDetailsService.Add(productDetailsEntity);

                        productDetailsIdPrivate = productDetailsEntity.ProductDetailsId;
                    }   

                    // 4. ProductStocks ekleme
                    var productStocks = new ProductStocks
                    {
                        ProductDetailsId = productDetailsIdPrivate,
                        ProductColorId = productStockAddDto.ProductColorId,
                        UnitsInStock = productStockAddDto.UnitsInStock,
                        Status = productStockAddDto.Status,
                        Images = productStockAddDto.Images
                    };

                    _productStockService.Add(productStocks);

                    // 5. İşlemleri ta
                    // mamla
                    transaction.Complete();
                    return new SuccessResult("Ürün ve detayları başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    // Hata durumunda geriye hata mesajı döner
                    return new ErrorResult($"Ürün eklenirken bir hata oluştu: {ex.Message}");
                }
            }
        }

        [CacheAspect] //key,value
        [PerformanceAspect(5000)]
        [SecuredOperation("product.add,admin")]
        public IDataResult<List<Product>> GetAllForAdmin()
        {
            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll(), Messages.ProductsListed);
        }
        [CacheAspect]
        [PerformanceAspect(5000)]
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll().Where(p => p.Status == true).ToList(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            var result = new SuccessDataResult<List<Product>>(GetAllForAdmin().Data.Where(p => p.CategoryId == id).ToList());
            if (result.Success) return result;
            return new SuccessDataResult<List<Product>>(GetAll().Data.Where(p => p.CategoryId == id).ToList());
        }

        [CacheAspect]
        [SecuredOperation("product.add,admin")]
        public IDataResult<Product> GetByIdForAdmin(int id)
        {
            return new SuccessDataResult<Product>(_ProductDal.Get(p => p.ProductId == id));
        }
        
        [CacheAspect]
        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_ProductDal.Get(p => p.ProductId == id));
        }

        
        [SecuredOperation("product.add,admin")]
        public IDataResult<List<ProductDetailDto>> GetProductDetailsForAdmin()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_ProductDal.GetProductDetails());
        }

        [SecuredOperation("product.add,admin")]
        public IDataResult<List<ProductDto>> GetByProductCodeForProductDto(string productCode)
        {
            return new SuccessDataResult<List<ProductDto>>(_ProductDal.GetByProductCodeForProductDto(productCode));
        }

        [SecuredOperation("product.add,admin")]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product, ProductDetails productDetails, ProductStocks productStocks)
        {
            // Ürün güncellenmek isteniyor
            var productToUpdate = _ProductDal.Get(p => p.ProductId == product.ProductId);
            if (productToUpdate == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            // Yalnızca güncellenebilir alanları kontrol ediyoruz
            productToUpdate.ProductName = product.ProductName ?? productToUpdate.ProductName;
            productToUpdate.ProductDescription = product.ProductDescription ?? productToUpdate.ProductDescription;
            productToUpdate.UnitPrice = product.UnitPrice != 0 ? product.UnitPrice : productToUpdate.UnitPrice;
            productToUpdate.CategoryId = product.CategoryId != 0 ? product.CategoryId : productToUpdate.CategoryId;

            _ProductDal.Update(productToUpdate); // Product güncellemesi

            var matchingProductDetailsList = _productDetailsService
                        .GetAllByProductIdAndSize(product.ProductId, productDetails.ProductSize);

            if (productStocks.ProductColorId == _productStockService.GetById(productStocks.ProductStocksId).Data.ProductColorId &&
                productDetails.ProductSize == _productDetailsService.GetById(productDetails.ProductDetailsId).Data.ProductSize)
            {

            }
            else 
            {
                foreach (var productDetails1 in matchingProductDetailsList)
                {
                    // Bu ProductDetailsId ile aynı renk olup olmadığını kontrol et
                    var existingProductStocks = _productStockService.GetAllByProductDetailsIdAndColor(
                        productDetails1.ProductDetailsId,
                        productStocks.ProductColorId
                    );

                    if (existingProductStocks != null && existingProductStocks.Any())
                    {
                        // Aynı renk için stok bulundu, işlem iptal
                        return new ErrorResult("Bu ürün boyutu ve rengi için stok zaten mevcut.");
                    }
                }
            }

            // Ürün Detayları
            var productDetailsToUpdate = _ProductDal.GetProductDetailsById(productDetails.ProductDetailsId);
            if (productDetailsToUpdate != null)
            {
                productDetailsToUpdate.ProductSize = productDetails.ProductSize ?? productDetailsToUpdate.ProductSize;
                _ProductDal.UpdateProductDetails(productDetailsToUpdate); // ProductDetails güncellemesi
            }

            // Ürün Stokları
            var productStocksToUpdate = _ProductDal.GetProductStockById(productStocks.ProductStocksId);
            if (productStocksToUpdate != null)
            {
                productStocksToUpdate.UnitsInStock = productStocks.UnitsInStock != 0 ? productStocks.UnitsInStock : productStocksToUpdate.UnitsInStock;
                productStocksToUpdate.Status = true;
                productStocksToUpdate.ProductColorId = productStocks.ProductColorId != 0 ? productStocks.ProductColorId : productStocksToUpdate.ProductColorId;
                productStocksToUpdate.Images = productStocks.Images ?? productStocksToUpdate.Images;

                // ColorId ekle
                productStocksToUpdate.ProductColorId = productStocks.ProductColorId != 0 ? productStocks.ProductColorId : productStocksToUpdate.ProductColorId;

                _ProductDal.UpdateProductStocks(productStocksToUpdate); // ProductStocks güncellemesi
            }

            return new SuccessResult(Messages.ProductUpdated);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            //Select count(*) from products where categoryId = 1
            var result = _ProductDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _ProductDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductCodeExists(string productCode)
        {
            var existingProduct = _ProductDal.Get(p => p.ProductCode == productCode);
            if (existingProduct != null)
            {
                return new ErrorResult("Ürün kodu mevcut");
            }
            return new SuccessResult();
        }


        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        /*
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if (product.UnitPrice<10)
            {
                throw new Exception("Birim Fiyatı 10 dan düşük olamaz"); // Bu satır hata fırlatırsa tüm işlemler geri alınır
            }
            Add(product);
            return new SuccessResult("Ürün bir işlem içinde başarıyla eklendi");
        }
        */

        [SecuredOperation("product.add,admin")]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Delete(int productID)
        {
            var result = _ProductDal.Get(p => p.ProductId == productID);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            _ProductDal.Delete(result);
            return new SuccessResult(result.ProductName + Messages.ProductDeleted);
        }
        
        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            var productDetails = _ProductDal.GetProductDetails();
            return new SuccessDataResult<List<ProductDetailDto>>(productDetails);
        }        
        
        public IDataResult<List<ProductDetailDto2>> GetProductDetails2()
        {
            var productDetails = _ProductDal.GetProductDetails2();
            return new SuccessDataResult<List<ProductDetailDto2>>(productDetails);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetailsWithFilters(ProductFilterModel filter)
        {
            var result = _ProductDal.GetProductDetailsWithFilters(filter);
            return new SuccessDataResult<List<ProductDetailDto>>(result, Messages.ProductsFiltered);
        }
        

        public IResult Restore(int productID)
        {
            var result = _ProductDal.Get(p => p.ProductId == productID);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            _ProductDal.Restore(result);
            return new SuccessResult(result.ProductName + Messages.Restored);

        }
        
        public IDataResult<List<ProductDetailDto>> GetProductStockDetails(int productStockId)
        {
            var productDetails = _ProductDal.GetProductDetails()
                                        .Where(p => p.ProductStocksId == productStockId)
                                        .ToList();

            // Veritabanından alınan veriyi DTO'ya dönüştürme (varsa)
            return new SuccessDataResult<List<ProductDetailDto>>(productDetails);
        }

        public IDataResult<List<ProductDetailDto>> GetProductStockDetailsByProduct(int productId)
        {
            var productDetails = _ProductDal.GetProductDetails().Where(p => p.ProductId == productId).ToList();

            if(productDetails != null) return new SuccessDataResult<List<ProductDetailDto>>(productDetails);

            return new ErrorDataResult<List<ProductDetailDto>>("Veri bulunamadı");
        }

        public IDataResult<List<ProductDto>> GetProductDto()
        {
            var productDto = _ProductDal.GetProductDto();
            return new SuccessDataResult<List<ProductDto>>(productDto);
        }

        public IDataResult<List<ProductWithTotalStockDto>> GetProductsWithTotalStock()
        {
            var result = _ProductDal.GetProductsWithTotalStock();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductWithTotalStockDto>>(result, "Ürünler başarıyla yüklendi.");
            }
            return new ErrorDataResult<List<ProductWithTotalStockDto>>("Ürünler yüklenirken bir hata oluştu.");
        }

        public IDataResult<List<ProductDetailDto2>> GetByCategoryIdProductDetails2(int superCategoryId, int? categoryId)
        {
            var productDetails = _ProductDal.GetByCategoryIdProductDetails2(superCategoryId, categoryId);
            return new SuccessDataResult<List<ProductDetailDto2>>(productDetails);
        }

        public IDataResult<List<ProductDetailDto2>> GetByProductIdForProductDetails2(int productId)
        {
            var productDetails = _ProductDal.GetByProductIdForProductDetails2(productId);
            return new SuccessDataResult<List<ProductDetailDto2>>(productDetails);
        }
    }
}

   