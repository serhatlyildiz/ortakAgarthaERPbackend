using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserOperationClaim.Get")]
        public IResult Add(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult(Messages.UserDeleted);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserOperationClaim.Get")]
        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Delete(userOperationClaim);
            return new SuccessResult(Messages.UserOperationClaimDeleted);
        }

        [SecuredOperation("admin")]
        [CacheAspect]
        public IDataResult<List<UserOperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll(), Messages.OperationClaimListed);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserOperationClaim.Get")]
        public IDataResult<List<UserOperationClaim>> GetAllByUserId(int userId)
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll(u => u.UserId == userId));
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserOperationClaim.Get")]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Update(userOperationClaim);
            return new SuccessResult(Messages.UserOperationClaimUpdated);
        }
    }
}