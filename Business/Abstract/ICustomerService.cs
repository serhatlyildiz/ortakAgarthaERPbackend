using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICustomerService
    {
        IDataResult<List<Customer>> GetAll();
        IDataResult<List<Customer>> GetAllByCustomerId(int customerID);
        IDataResult<Customer> GetById(string customerEmail);
        IResult Add(Customer customer);
        IResult Update(Customer customer);
        IResult Delete(int customerID);
    }
}
