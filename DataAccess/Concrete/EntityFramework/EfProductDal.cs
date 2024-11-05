using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
        private readonly IMapper _mapper;

        public EfProductDal(IMapper mapper)
        {
            _mapper = mapper;
        }
        //public List<ProductDetailDto> GetProductDetails()
        //{
        //    using (NorthwindContext context = new NorthwindContext())
        //    {
        //        var result = from p in context.Products
        //                     join c in context.Categories
        //                     on p.CategoryId equals c.CategoryId
        //                     select new ProductDetailDto
        //                     {
        //                         ProductId = p.ProductId,
        //                         ProductName = p.ProductName,
        //                         CategoryName = c.CategoryName,
        //                         UnitInStock = p.UnitsInStock,
        //                     };
        //        return result.ToList();
        //    }
        //}

        public List<ProductDetailDto> GetProductDetails()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var result = context.Products
                            .ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider)
                            .ToList();
                return result;
            }
        }
    }
}
