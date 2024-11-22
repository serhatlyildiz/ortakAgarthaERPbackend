using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IillerService
    {
        IDataResult<List<iller>> GetAll();
        IDataResult<iller> GetById(int ilId);
    }
}