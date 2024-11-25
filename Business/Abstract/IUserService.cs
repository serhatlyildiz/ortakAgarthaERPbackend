using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(Users user);
        public void Add(Users user);
        IResult Update(Users user);
        IResult Update(UserForUpdateDto user);
        IResult Delete(int userID);
        IDataResult<List<Users>> GetAll();
        Users GetByMail(string email);
        Users GetById(int id);
    }
}