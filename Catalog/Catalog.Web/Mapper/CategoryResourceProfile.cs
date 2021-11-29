using AutoMapper;
using Catalog.Service.Models;

namespace Catalog.Web.Mapper
{
	public class CategoryResourceProfile : Profile
    {
        public CategoryResourceProfile()
        {
            CreateMap<CategoryModel, CategoryResource>();
        }
    }
}
