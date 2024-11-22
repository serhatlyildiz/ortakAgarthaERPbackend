using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISuperCategoryService
    {
        IDataResult<List<SuperCategory>> GetAll();
        IResult Add(SuperCategory superCategory);
        IResult Update(SuperCategory superCategory);
        IResult Delete(int superCategoryId);
        IDataResult<SuperCategory> GetById(int superCategoryId);
    }
}