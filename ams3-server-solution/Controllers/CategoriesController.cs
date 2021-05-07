
using Ams3.Controllers;
using Ams3.Models;
using Ams3.Utility;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ams2.Controllers {

	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : AmsWebApiController {

		public CategoriesController(AmsDbContext context) : base(context) { }

		[HttpGet("List")]
		public async Task<ActionResult<JsonResponse>> GetCategories() {
			return new JsonResponse { Data = await db.Categories.ToListAsync() };
		}

		[HttpGet("Get/{id}")]
		public async Task<ActionResult<JsonResponse>> GetCategory(int? id) {
			if (id == null)
				return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
			var Category = await db.Categories.FindAsync(id);
			if (Category == null)
				return new JsonResponse { Code = -2, Message = $"Category id={id} not found" };
			return new JsonResponse(Category);
		}

		[HttpPost("Create")]
		public async Task<ActionResult<JsonResponse>> AddCategory(Category Category) {
			if (Category == null)
				return new JsonResponse { Code = -2, Message = "Parameter Category cannot be null" };
			db.Categories.Add(Category);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Category Created", Data = Category };
		}

		[HttpPost("Change")]
		public async Task<ActionResult<JsonResponse>> ChangeCategory(Category Category) {
			if (Category == null)
				return new JsonResponse { Code = -2, Message = "Parameter Category cannot be null" };
			db.Entry(Category).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Category Changed", Data = Category };
		}

		[HttpPost("Remove")]
		public async Task<ActionResult<JsonResponse>> RemoveCategory(Category category) {
			if (category == null)
				return new JsonResponse { Code = -2, Message = "Parameter Category cannot be null" };
			db.Categories.Remove(category);
			await db.SaveChangesAsync();
			return  new JsonResponse { Message = "Category Removed", Data = category };
		}

		//private JsonResponse SaveChanges(JsonResponse resp = null) {
		//	try {
		//		db.SaveChanges();
		//		return resp ?? JsonResponse.Ok;
		//	} catch (Exception ex) {
		//		return new JsonResponse { Message = ex.Message, Error = ex };
		//	}
		//}
	}
}
