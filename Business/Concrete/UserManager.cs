using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.DTOs;
using System.Linq;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        IOperationClaimDal _operationClaimDal;

        public UserManager(IUserDal userDal, IOperationClaimDal operationClaimDal)
        {
            _userDal = userDal;
            _operationClaimDal = operationClaimDal;
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

            _userDal.Delete(result);
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

        public IResult UpdateForAdmin(UserForUpdateDto userDto)
        {
            var userToUpdate = _userDal.Get(u => u.Id == userDto.Id);

            if (userToUpdate == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            if (!string.IsNullOrWhiteSpace(userDto.FirstName))
                userToUpdate.FirstName = userDto.FirstName;

            if (!string.IsNullOrWhiteSpace(userDto.LastName))
                userToUpdate.LastName = userDto.LastName;

            if (!string.IsNullOrWhiteSpace(userDto.Email))
                userToUpdate.Email = userDto.Email;

            if (!string.IsNullOrWhiteSpace(userDto.City))
                userToUpdate.City = userDto.City;

            if (!string.IsNullOrWhiteSpace(userDto.District))
                userToUpdate.District = userDto.District;

            if (!string.IsNullOrWhiteSpace(userDto.Adress))
                userToUpdate.Adress = userDto.Adress;

            if (!string.IsNullOrWhiteSpace(userDto.Adress))
                userToUpdate.Cinsiyet = userDto.Cinsiyet;

            userToUpdate.Status = userDto.Status;
            userToUpdate.Roles = userDto.Roles;

            _userDal.Update(userToUpdate);

            return new SuccessResult(Messages.UserUpdated);
        }

        public IDataResult<List<Users>> GetFilteredUsers(UserFilterDto filter)
        {
            var query = _userDal.GetAll().AsQueryable(); // Başlangıçta tüm kullanıcıları alıyoruz

            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(filter.Firstname))
            {
                string lowerCaseName = filter.Firstname.ToLower();
                query = query.Where(u => u.FirstName.ToLower().Contains(lowerCaseName));
            }

            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                string lowerCaseName = filter.LastName.ToLower();
                query = query.Where(u => u.LastName.ToLower().Contains(lowerCaseName));
            }

            if (!string.IsNullOrEmpty(filter.City))
            {
                query = query.Where(u => u.City.Equals(filter.City));
            }

            if (!string.IsNullOrEmpty(filter.District))
            {
                query = query.Where(u => u.District.Equals(filter.District));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(u => u.Status == filter.Status.Value); // Status değerini kontrol ediyoruz
            }

            if (!string.IsNullOrEmpty(filter.Gender))
            {
                query = query.Where(u => u.Cinsiyet.Equals(filter.Gender, StringComparison.OrdinalIgnoreCase)); // Cinsiyet filtresi
            }

            if (!string.IsNullOrEmpty(filter.Role))
            {
                if (filter.Role == "0")
                {
                    // Eğer "0" ise, rolü olmayan kullanıcıları getirin
                    query = query.Where(u => !u.Roles.Any());
                }
                else
                {
                    int roleId;
                    if (int.TryParse(filter.Role, out roleId))
                    {
                        // Eğer geçerli bir rol ID'si ise, o rolü olan kullanıcıları getirin
                        query = query.Where(u => u.Roles.Contains(roleId));
                    }
                }
            }

            // Filtrelenmiş kullanıcıları listeye dönüştürüyoruz
            var users = query.ToList();

            // Sonuç döndürülüyor
            return new SuccessDataResult<List<Users>>(users, "Users filtered successfully.");
        }

        public IDataResult<UserForUpdateDto> GetByIdAdmin(int id)
        {
            return new SuccessDataResult<UserForUpdateDto>(_userDal.GetAllForUpdates().Find(u => u.Id == id));
        }

        public IResult Restore(int Id)
        {
            var result = _userDal.Get(p => p.Id == Id);

            if (result == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            _userDal.Restore(result);
            return new SuccessResult(result.FirstName + Messages.Restored);
        }
    }
}