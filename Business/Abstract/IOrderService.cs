﻿using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IDataResult<List<Order>> GetAll();
        IDataResult<List<Order>> GetAllByUserId(int orderUserID);
        IDataResult<List<Order>> GetAllByProductId(int orderProductID);
        IDataResult<List<Order>> GetAllByDate(DateTime orderDate);
        IDataResult<Order> GetById(int orderID);
        IResult Add(Order order);
        IResult Update(Order order);
        IResult Delete(int orderID);
    }
}