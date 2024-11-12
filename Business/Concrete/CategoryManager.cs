using Business.Abstract;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            IResult result = BusinessRules.Run(CheckIfProductNameExists(category.CategoryName));

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
            // Güncellenmek istenen ürünün veritabanında mevcut olup olmadığını kontrol et
            var result = _categoryDal.Get(c => c.CategoryId == categoryID);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer ürün yoksa hata döndür
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

        public IResult Update(Category category)
        {
            var result = _categoryDal.Get(c => c.CategoryId == category.CategoryId);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer ürün yoksa hata döndür
            }

            _categoryDal.Update(category);
            return new SuccessResult(Messages.CategoryUpdated); // Başarı mesajı döndür
        }

        private IResult CheckIfProductNameExists(string categoryName)
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
