using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IilcelerService
    {
        IDataResult<List<ilceler>> GetAllByIllerId(int ilId);

        IDataResult<ilceler> GetById(int ilcelerId);
    }
}
