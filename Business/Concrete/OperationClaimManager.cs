﻿using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
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
    public class OperationClaimManager : IOperationClaimService
    {
        IOperationClaimDal _operationClaimDal;

        public  OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        public IResult Add(OperationClaim operationClaim)
        {
            var result = _operationClaimDal.Get(oc => oc.Id == operationClaim.Id);

            if (result == null)
            {
                return new ErrorResult(Messages.OperationClaimNotFound); // Eğer rol yoksa hata döndür
            }
;
            _operationClaimDal.Add(result);
            return new SuccessResult(Messages.OperationClaimAdded);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        public IResult Delete(OperationClaim operationClaim)
        {
            var result = _operationClaimDal.Get(oc => oc.Id == operationClaim.Id);

            if (result == null)
            {
                return new ErrorResult(Messages.OperationClaimNotFound); // Eğer rol yoksa hata döndür
            }
;
            _operationClaimDal.Delete(result);
            return new SuccessResult(Messages.OperationClaimDeleted);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        public IResult Update(OperationClaim operationClaim)
        {
            var result = _operationClaimDal.Get(oc => oc.Id == operationClaim.Id);

            if (result == null)
            {
                return new ErrorResult(Messages.OperationClaimNotFound); // Eğer rol yoksa hata döndür
            }
;
            _operationClaimDal.Delete(result);
            return new SuccessResult(Messages.OperationClaimUpdated);
        }

        
        [SecuredOperation("admin")]
        [CacheAspect]
        public IDataResult<List<OperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll(), Messages.OperationClaimListed);
        }
        
    }
}
