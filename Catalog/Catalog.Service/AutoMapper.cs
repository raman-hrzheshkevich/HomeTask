using AutoMapper;
using Catalog.DataAccess.Models;
using Catalog.Service.Models;
using System;

namespace Catalog.Service
{
	public static class AutoMapper
	{

		private static readonly Lazy<IMapper> Mapper = new Lazy<IMapper>(() =>
		{
			MapperConfiguration mapperConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Product, ProductModel>().ReverseMap();
				config.CreateMap<Category, CategoryModel>().ReverseMap();
			});

			return new Mapper(mapperConfig);
		});

		public static TDestination Map<TDestination>(object source)
		{
			return Mapper.Value.Map<TDestination>(source);
		}
	}
}
