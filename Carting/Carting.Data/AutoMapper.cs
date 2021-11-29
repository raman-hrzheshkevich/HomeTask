using AutoMapper;
using Carting.Data.Services.Models;
using Carting.DataAccess.LiteDb.Models;
using System;

namespace Carting.Data
{
	public static class AutoMapper
	{

		private static readonly Lazy<IMapper> Mapper = new Lazy<IMapper>(() =>
		{
			MapperConfiguration mapperConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Cart, CartModel>()
					.ForMember(c => c.CartItems, opts => opts.MapFrom(s => s.Items))
					.ReverseMap();
				config.CreateMap<CartImage, CartImageModel>().ReverseMap();
				config.CreateMap<CartItem, CartItemModel>().ReverseMap();
			});

			return new Mapper(mapperConfig);
		});

		public static TDestination Map<TDestination>(object source) 
		{
			return Mapper.Value.Map<TDestination>(source);
		}
	}
}
