using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IDataResult<List<Order>> GetAll();
        IDataResult<List<Order>> GetAllByCustomerId(int orderCustomerID);
        IDataResult<List<Order>> GetAllByProductId(int orderProductID);
        IDataResult<List<Order>> GetAllByDate(DateTime orderDate);
        IDataResult<Order> GetById(int orderID);
        IResult Add(Order order);
        IResult Update(Order order);
        IResult Delete(int orderID);
    }
}
