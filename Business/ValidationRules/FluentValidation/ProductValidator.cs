﻿using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            //RuleFor(p => p.ProductName).NotEmpty();
            //RuleFor(p => p.ProductName).MinimumLength(2);
            //RuleFor(p => p.UnitPrice).NotEmpty();
            //RuleFor(p => p.UnitPrice).GreaterThan(0);
            //RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
            //RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı");
            //RuleFor(p => p.UnitsInStock).Must(asdada).WithMessage("Stok 5 den az");
        }

        /*
        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
        

        private bool asdada(short arg)
        {
            if (arg <= 5) {
                return false;
            }
            return true;
        }
        */
    }
}