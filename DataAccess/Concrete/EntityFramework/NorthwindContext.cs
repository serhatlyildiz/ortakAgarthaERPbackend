using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class NorthwindContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Northwind;Trusted_Connection=True");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Colors> Colors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<Users> Users { get; set; }
        //public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<SuperCategory> SuperCategories { get; set; }
        public DbSet<ProductStocks> ProductStocks { get; set; }
        public DbSet<iller> iller { get; set; }
        public DbSet<ilceler> ilceler { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        public DbSet<ProductStatusHistory> ProductStatusHistories { get; set; }
    }
}