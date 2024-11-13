using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
