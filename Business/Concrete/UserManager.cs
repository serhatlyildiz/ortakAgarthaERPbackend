using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.DTOs;

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
            var userToUpdate = _userDal.Get(u => u.Id == user.Id);

            if (userToUpdate == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            // Durumu kontrol et ve kullanıcıyı güncelle
            userToUpdate.Status = user.Status;

            _userDal.Update(userToUpdate);
            return new SuccessResult(Messages.UserUpdated);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(int Id)
        {
            var result = _userDal.Get(p => p.Id == Id);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }
            if (result.Status) result.Status = false;
            else result.Status = true;

            _userDal.Update(result);
            return new SuccessResult(result.FirstName + " " + result.LastName + Messages.UserDeleted);
        }

        public IDataResult<List<Users>> GetAll()
        {
            return new SuccessDataResult<List<Users>>(_userDal.GetAll(), Messages.ProductsListed);
        }

        public Users GetById(int id)
        {
            return _userDal.Get(u => u.Id == id);
        }

        public IDataResult<List<UserWithRolesDto>> GetAllWithRoles()
        {
            var usersWithRoles = _userDal.GetAllWithRoles();
            return new SuccessDataResult<List<UserWithRolesDto>>(usersWithRoles);
        }

    }
}