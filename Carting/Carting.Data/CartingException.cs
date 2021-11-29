using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carting.Data
{
	public class CartingException : Exception
	{
		public CartingException(string message) : base(message)
		{
		}
	}
}
