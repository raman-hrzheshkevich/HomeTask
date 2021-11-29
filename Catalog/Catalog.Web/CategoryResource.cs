using AutoMapper;
using Catalog.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;

namespace Catalog.Web
{
	public class CategoryResource : CategoryModel
	{
		private readonly IUrlHelper urlHelper;
		private readonly IMapper mapper;
		private readonly List<ResourceLink> _links = new List<ResourceLink>();

		public CategoryResource()
		{

		}

		public CategoryResource(IUrlHelper urlHelper, IMapper mapper)
		{
			this.urlHelper = urlHelper;
			this.mapper = mapper;
		}

		public IEnumerable<ResourceLink> Links => _links;

		public CategoryResource AddLink(ResourceLink resourceLink)
		{
			_links.Add(resourceLink);
			return this;
		}


		public CategoryResource Create(CategoryModel categoryModel)
		{
			CategoryResource categoryResource = mapper.Map<CategoryResource>(categoryModel);

			var x = urlHelper.Link("GetCategories", null);

			categoryResource.AddLink(new ResourceLink { Method = HttpMethod.Get.Method, Rel = "all", Href = urlHelper.ActionLink("GetCategories", "Category" , new { }) });
			categoryResource.AddLink(new ResourceLink { Method = HttpMethod.Delete.Method, Rel = "delete-category", Href = urlHelper.ActionLink("DeleteCategory", "Category", new { categoryId = categoryModel.CategoryId }) });
			categoryResource.AddLink(new ResourceLink { Method = HttpMethod.Put.Method, Rel = "update-category", Href = urlHelper.ActionLink("UpdateCategory", "Category", new { categoryId = categoryModel.CategoryId }) });
			categoryResource.AddLink(new ResourceLink { Method = HttpMethod.Post.Method, Rel = "create-category", Href = urlHelper.ActionLink("CreateCategory", "Category", new { }) });

			return categoryResource;
		}
	}
}
