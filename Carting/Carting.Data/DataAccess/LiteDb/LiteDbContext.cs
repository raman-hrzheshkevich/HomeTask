using LiteDB;
using Microsoft.Extensions.Options;
using System;

namespace Carting.DataAccess.LiteDb
{
	public class LiteDbContext : ILiteDbContext, IDisposable
	{
		private bool _disposed;
		public ILiteDatabase Context { get; }

		public LiteDbContext(string connectionString)
		{
			try
			{
				var db = new LiteDatabase(connectionString);
				if (db != null)
				{
					Context = db;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can find or create LiteDb database.", ex);
			}
		}

		public LiteDbContext(IOptions<LiteDbOptions> options) : this (options.Value.DatabaseLocation)
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (Context != null)
						Context.Dispose();
				}
				_disposed = true;
			}
		}

		~LiteDbContext()
		{
			Dispose(false);
		}
	}
}
