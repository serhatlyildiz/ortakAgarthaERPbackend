using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IColorsService
    {
        IDataResult<List<Colors>> GetAll();
        IResult Add(Colors colors);
        IResult Update(Colors colors);
        IResult Delete(int colorId);
        IDataResult<Colors> GetById(int colorId);
    }
}