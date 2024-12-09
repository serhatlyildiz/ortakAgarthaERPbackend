using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetAll();
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Delete(int categoryID);
        IDataResult<List<Category>> GetBySuperCategoryId(int superCategoryId);
        IDataResult<Category> GetById(int categoryId);
    }
}