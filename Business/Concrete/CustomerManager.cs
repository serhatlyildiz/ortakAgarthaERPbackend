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
    public class CustomerManager : ICustomerService
    {
        ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        public IResult Add(Customer customer)
        {
            IResult result = BusinessRules.Run(CheckIfCustomerEmailExists(customer.CustomerEmail));

            if (result != null)
            {
                return result;
            }

            _customerDal.Add(customer);

            return new SuccessResult(Messages.CustomerAdded);
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        public IResult Delete(int customerID)
        {
            var result = _customerDal.Get(c => c.CustomerId == customerID);

            if (result == null)
            {
                return new ErrorResult(Messages.CustomerNotFound); // Eğer ürün yoksa hata döndür
            }

            // Ürünü sil
            string silinenKullanici = result.CustomerFirstName + " " + result.CustomerFirstName;
            _customerDal.Delete(result);
            return new SuccessResult(silinenKullanici + Messages.CustomerDeleted);
        }

        [CacheAspect] //key,value
        [PerformanceAspect(5000)]
        public IDataResult<List<Customer>> GetAll()
        {
            return new SuccessDataResult<List<Customer>>(_customerDal.GetAll(), Messages.CustomersListed);
        }

        public IDataResult<List<Customer>> GetAllByCustomerId(int customerID)
        {
            return new SuccessDataResult<List<Customer>>(_customerDal.GetAll(c => c.CustomerId == customerID));
        }

        public IDataResult<Customer> GetById(string customerEmail)
        {
            return new SuccessDataResult<Customer>(_customerDal.Get(c => c.CustomerEmail == customerEmail));
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        public IResult Update(Customer customer)
        {
            var productToUpdate = _customerDal.Get(c => c.CustomerId == customer.CustomerId);

            if (productToUpdate == null)
            {
                return new ErrorResult(Messages.ProductNotFound); // Eğer ürün yoksa hata döndür
            }

            IResult result = BusinessRules.Run(CheckIfCustomerEmailExists(customer.CustomerEmail));

            if (result != null)
            {
                return result;
            }

            // Ürünün güncellenmesini sağla
            _customerDal.Update(customer);
            return new SuccessResult(Messages.CustomerUpdated); // Başarı mesajı döndür
        }

        private IResult CheckIfCustomerEmailExists(string CustomerEmail)
        {
            var result = _customerDal.GetAll(c => c.CustomerEmail == CustomerEmail).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
