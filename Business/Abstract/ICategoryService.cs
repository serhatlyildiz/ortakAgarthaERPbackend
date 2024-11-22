using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetAll();
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Delete(int categoryID);
        IDataResult<Category> GetById(int categoryId);
    }
}