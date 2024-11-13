using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
