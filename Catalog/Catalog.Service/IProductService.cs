using Catalog.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Service
{
	public interface IProductService
	{
		Task<ProductModel> AddProduct(ProductModel productModel);
		Task DeleteProduct(int productId);
		Task<IEnumerable<ProductModel>> GetProducts(ProductsQuery query);
		Task UpdateProduct(int productId, ProductModel productModel);
	}
}