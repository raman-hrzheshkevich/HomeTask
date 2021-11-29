using Catalog.DataAccess;
using Catalog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Catalog.IntegrationTests
{

	public class TestCategoryContext : CategoryContext
	{
		public TestCategoryContext()
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		public TestCategoryContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=testDB;Trusted_Connection=True;");
		}
	}
}
