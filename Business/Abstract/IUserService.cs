using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(Users user);
        public void Add(Users user);
        IResult Update(Users user);
        IResult Delete(int userID);

        IDataResult<List<Users>> GetAll();
        Users GetByMail(string email);
    }
}
