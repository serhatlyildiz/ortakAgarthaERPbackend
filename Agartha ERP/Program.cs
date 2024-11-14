using Business.Concrete;
using Castle.Core.Configuration;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
//using DataAccess.Concrete.InMemory;
using DataAccess.Mapping;
using Entities.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace ConsoleUI
{
    //SOLID
    //Open Closed Principle
    internal class Program
    {
        static void Main(string[] args)
        {
            //ProductTest();
            //CategoryTest();

            var serviceProvider = new ServiceCollection()
                .AddAutoMapper(typeof(MappingProfile)) // AutoMapper'ı ekleyin
                .AddScoped<IProductDal, EfProductDal>() // EfProductDal'ı ekleyin
                .AddScoped<ICategoryDal, EfCategoryDal>() // EfCategoryDal'ı ekleyin
                .AddScoped<ProductManager>() // ProductManager'ı ekleyin
                .AddScoped<CategoryManager>() // CategoryManager'ı ekleyin
                .BuildServiceProvider(); // Servis sağlayıcıyı oluşturun

            // Test metotlarını çağırın
            ProductTest(serviceProvider);
            CategoryTest(serviceProvider);
        }

        private static void CategoryTest(IServiceProvider serviceProvider)
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest(IServiceProvider serviceProvider)
        {
            //ProductManager productManager = new ProductManager(new EfProductDal(),
            //        new CategoryManager(new EfCategoryDal()));

            var productManager = serviceProvider.GetService<ProductManager>();

            var result = productManager.GetProductDetails();

            if (result.Success == true)
            {
                foreach (var product in result.Data)
                {
                    Console.WriteLine(product.ProductName + "/" + product.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }
    }
}
//durali

//yenibranç denemesi