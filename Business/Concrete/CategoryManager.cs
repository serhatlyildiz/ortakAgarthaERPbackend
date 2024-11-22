using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        [SecuredOperation("category.add,admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Add(Category category)
        {
            IResult result = BusinessRules.Run(CheckIfCategoryNameExists(category.CategoryName));

            if (result != null)
            {
                return result;
            }

            _categoryDal.Add(category);

            return new SuccessResult(Messages.CategoryAdded);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Delete(int categoryID)
        {
            // Güncellenmek istenen kategori veritabanında mevcut olup olmadığını kontrol et
            var result = _categoryDal.Get(c => c.CategoryId == categoryID);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer kategori yoksa hata döndür
            }

            _categoryDal.Delete(result);
            return new SuccessResult(Messages.CategoryDeleted); // Başarı mesajı döndür
        }

        public IDataResult<List<Category>> GetAll()
        {
            //İş Kodları
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll());
        }

        //Select * from Categories where CategoryId = 3
        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>(_categoryDal.Get(c => c.CategoryId == categoryId));
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Update(Category category)
        {
            var result = _categoryDal.Get(c => c.CategoryId == category.CategoryId);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer kategori yoksa hata döndür
            }

            _categoryDal.Update(category);
            return new SuccessResult(Messages.CategoryUpdated); // Başarı mesajı döndür
        }

        private IResult CheckIfCategoryNameExists(string categoryName)
        {
            var result = _categoryDal.GetAll(c => c.CategoryName == categoryName).Any();
            if (result)
            {
                return new ErrorResult(Messages.CategoryNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}