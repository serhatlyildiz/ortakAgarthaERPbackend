using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IilcelerService
    {
        IDataResult<List<ilceler>> GetAllByIllerId(int ilId);
        IDataResult<ilceler> GetById(int ilcelerId);
    }
}