using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _ProductDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _ProductDal = productDal;
            _categoryService = categoryService;
        }

        //Claim
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {

            //business codes

            //Kural 1 - Aynı isimde ürün eklenemez
            //Kural 2 - 10dan fazla aynı kategoride ürün eklenememsin
            //Kural 3 - Eğer mevcut kategori sayısı 15'i geçtiyse yeni ürün eklenemez


            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _ProductDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
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
            return new SuccessDataResult<Product>(_ProductDal.Get(p => p.ProductId == id && p.Status == true));
        }


        [SecuredOperation("product.add,admin")]
        public IDataResult<List<ProductDetailDto>> GetProductDetailsForAdmin()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_ProductDal.GetProductDetails());
        }
        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_ProductDal.GetProductDetails().Where(p => p.IsActive == true).ToList());
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            // Güncellenmek istenen ürünün veritabanında mevcut olup olmadığını kontrol et
            var productToUpdate = _ProductDal.Get(p => p.ProductId == product.ProductId);

            if (productToUpdate == null)
            {
                return new ErrorResult(Messages.ProductNotFound); // Eğer ürün yoksa hata döndür
            }

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            // Ürünün güncellenmesini sağla
            _ProductDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated); // Başarı mesajı döndür
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

        //[SecuredOperation("product.add,admin")]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Delete(int productID)
        {
            var result = _ProductDal.Get(p => p.ProductId == productID);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            if (result.Status) result.Status = false;
            else result.Status = true;


            _ProductDal.Update(result);
            return new SuccessResult(result.ProductName + Messages.ProductDeleted);
        }
    }
}