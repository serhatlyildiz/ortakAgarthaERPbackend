using Business.Abstract;
using Business.Constants;
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
    public class illerManager : IillerService
    {
        IillerDal _illerDal;

        public illerManager(IillerDal illerDal)
        {
            _illerDal = illerDal;
        }

        public IDataResult<List<iller>> GetAll()
        {
            return new SuccessDataResult<List<iller>>(_illerDal.GetAll(),"");
        }

        public IDataResult<iller> GetById(int ilId)
        {
            return new SuccessDataResult<iller>(_illerDal.Get(p => p.ilNo == ilId));
        }
    }
}
