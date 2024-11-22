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
    public class SuperCategoryManager : ISuperCategoryService
    {
        ISuperCategoryDal _superCategoryDal;

        public SuperCategoryManager(ISuperCategoryDal superCategoryDal)
        {
            _superCategoryDal = superCategoryDal;
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("ISuperCategoryService.Get")]
        public IResult Add(SuperCategory superCategory)
        {
            IResult result = BusinessRules.Run(CheckIfSuperCategoryNameExists(superCategory.SuperCategoryName));

            if (result != null)
            {
                return result;
            }

            _superCategoryDal.Add(superCategory);

            return new SuccessResult(Messages.CategoryAdded);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("ISuperCategoryService.Get")]
        public IResult Delete(int superCategoryId)
        {
            var result = _superCategoryDal.Get(s => s.SuperCategoryId == superCategoryId);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer kategori yoksa hata döndür
            }

            _superCategoryDal.Delete(result);
            return new SuccessResult(Messages.CategoryDeleted); // Başarı mesajı döndür
        }

        public IDataResult<List<SuperCategory>> GetAll()
        {
            return new SuccessDataResult<List<SuperCategory>>(_superCategoryDal.GetAll());
        }

        public IDataResult<SuperCategory> GetById(int superCategoryId)
        {
            return new SuccessDataResult<SuperCategory>(_superCategoryDal.Get(s => s.SuperCategoryId == superCategoryId));
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("ISuperCategoryService.Get")]
        public IResult Update(SuperCategory superCategory)
        {
            var result = _superCategoryDal.Get(s => s.SuperCategoryId == superCategory.SuperCategoryId);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer ürün yoksa hata döndür
            }

            _superCategoryDal.Update(superCategory);
            return new SuccessResult(Messages.CategoryUpdated); // Başarı mesajı döndür
        }

        private IResult CheckIfSuperCategoryNameExists(string superCategoryName)
        {
            var result = _superCategoryDal.GetAll(s => s.SuperCategoryName == superCategoryName).Any();
            if (result)
            {
                return new ErrorResult(Messages.CategoryNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}