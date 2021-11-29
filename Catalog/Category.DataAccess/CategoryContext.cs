using Catalog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess
{
	public class CategoryContext : DbContext
    {
		public CategoryContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=catalogDb;Trusted_Connection=True;");
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(new Category
			{
				CategoryId = 1,
				Image = "imageUrl",
				Name = "Category-1"
			});

			modelBuilder.Entity<Category>().HasData(new Category
			{
				CategoryId = 2,
				Name = "Category-2",
			});

			modelBuilder.Entity<Category>().HasData(new Category
			{
				CategoryId = 3,
				Name = "Category-1-1",
				ParentCategoryId = 1
			});

			modelBuilder.Entity<Product>().HasData(new Product { ProductId = 1, Name = "product-1-1", Amount = 1, Price = 1, CategoryId = 1 });
			modelBuilder.Entity<Product>().HasData(new Product { ProductId = 2, Name = "product-1-2", Amount = 1, Price = 1, CategoryId = 1 });

			modelBuilder.Entity<Product>().HasData(new Product { ProductId = 3, Name = "product-2-1", Amount = 1, Price = 1, CategoryId = 2 });

			modelBuilder.Entity<Product>().HasData(new Product { ProductId = 4, Name = "product-1-1-1", Amount = 3, Price = 3, CategoryId = 3 });
			modelBuilder.Entity<Product>().HasData(new Product { ProductId = 5, Name = "product-1-1-2", Amount = 4, Price = 4, CategoryId = 3 });
		}
	}
}
