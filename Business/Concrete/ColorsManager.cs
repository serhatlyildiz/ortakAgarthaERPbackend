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
    public class ColorsManager : IColorsService
    {
        IColorDal _colorsDal;

        public ColorsManager(IColorDal colorsService)
        {
            _colorsDal = colorsService;
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IColorsService.Get")]
        public IResult Add(Colors colors)
        {
            IResult result = BusinessRules.Run(CheckIfColorNameExists(colors.ColorName));

            if (result != null)
            {
                return result;
            }

            _colorsDal.Add(colors);

            return new SuccessResult(Messages.ColorAdded);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IColorsService.Get")]
        public IResult Delete(int colorId)
        {
            var result = _colorsDal.Get(c => c.ColorId == colorId);

            if (result == null)
            {
                return new ErrorResult(Messages.CategoryNotFound); // Eğer renk yoksa hata döndür
            }

            _colorsDal.Delete(result);
            return new SuccessResult(Messages.ColorDeleted); // Başarı mesajı döndür
        }

        public IDataResult<List<Colors>> GetAll()
        {
            return new SuccessDataResult<List<Colors>>(_colorsDal.GetAll());
        }

        public IDataResult<Colors> GetById(int colorId)
        {
            return new SuccessDataResult<Colors>(_colorsDal.Get(s => s.ColorId == colorId));
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IColorsService.Get")]
        public IResult Update(Colors colors)
        {
            var result = _colorsDal.Get(s => s.ColorId == colors.ColorId);

            if (result == null)
            {
                return new ErrorResult(Messages.ColorNotFound); // Eğer ürün yoksa hata döndür
            }

            _colorsDal.Update(colors);
            return new SuccessResult(Messages.ColorUpdated); // Başarı mesajı döndür
        }

        private IResult CheckIfColorNameExists(string colorName)
        {
            var result = _colorsDal.GetAll(c => c.ColorName == colorName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ColorNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}