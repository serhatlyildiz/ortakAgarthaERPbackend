using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
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
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaims(Users user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(Users user)
        {
            _userDal.Add(user);
        }

        public Users GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(Users user)
        {
            var userToUpdate = _userDal.Get(p => p.Id == user.Id);

            if (userToUpdate == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(int userID)
        {
            var userToDelete = _userDal.Get(u => u.Id == userID);

            if (userToDelete == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            _userDal.Delete(userToDelete);
            return new SuccessResult(Messages.UserDeleted);
        }

        public IDataResult<List<Users>> GetAll()
        {
            return new SuccessDataResult<List<Users>>(_userDal.GetAll(), Messages.ProductsListed);
        }
    }
}

