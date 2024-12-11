using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

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
            return new SuccessDataResult<List<iller>>(_illerDal.GetAll().OrderBy(i => i.ilNo).ToList(), "");
        }

        public IDataResult<iller> GetById(int ilId)
        {
            return new SuccessDataResult<iller>(_illerDal.Get(p => p.ilNo == ilId));
        }
    }
}