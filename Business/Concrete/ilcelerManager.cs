using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ilcelerManager : IilcelerService
    {
        IilcelerDal _ilcelerDal;
        IillerService _illerService;

        public ilcelerManager(IilcelerDal ilcelerDal, IillerService illerService)
        {
            _ilcelerDal = ilcelerDal;
            _illerService = illerService;
        }

        public IDataResult<List<ilceler>> GetAllByIllerId(int ilId)
        {
            return new SuccessDataResult<List<ilceler>>(_ilcelerDal.GetAll(i => i.ilno == ilId));
        }

        public IDataResult<ilceler> GetById(int ilcelerId)
        {
            return new SuccessDataResult<ilceler>(_ilcelerDal.Get(p => p.id == ilcelerId));
        }
    }
}
