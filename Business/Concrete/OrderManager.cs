using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        [CacheRemoveAspect("IOrderService.Get")]
        public IResult Add(Order order)
        {
            _orderDal.Add(order);

            return new SuccessResult(Messages.OrderAdded);
        }

        [CacheRemoveAspect("IOrderService.Get")]
        public IResult Delete(int orderID)
        {
            var result = _orderDal.Get(o => o.OrderId == orderID);
            _orderDal.Delete(result);
            return new SuccessResult(result.OrderId.ToString() + Messages.OrderDeleted);
        }

        [CacheAspect] //key,value
        [PerformanceAspect(5000)]
        public IDataResult<List<Order>> GetAll()
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Order>> GetAllByUserId(int orderUserID)
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.UserId == orderUserID));
        }

        public IDataResult<List<Order>> GetAllByDate(DateTime orderDate)
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.OrderDate == orderDate));
        }

        public IDataResult<List<Order>> GetAllByProductId(int orderProductID)
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.ProductId == orderProductID));
        }

        public IDataResult<Order> GetById(int orderID)
        {
            return new SuccessDataResult<Order>(_orderDal.Get(o => o.OrderId == orderID));
        }

        [CacheRemoveAspect("IOrderService.Get")]
        public IResult Update(Order order)
        {
            _orderDal.Update(order);
            return new SuccessResult(Messages.OrderUpdated); // Başarı mesajı döndür
        }
    }
}
