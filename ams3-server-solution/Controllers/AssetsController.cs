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
	public class AssetsController : AmsWebApiController {

		public AssetsController(AmsDbContext context) : base(context) { }

		[HttpGet("List")]
		public async Task<ActionResult<JsonResponse>> GetAssets() {
			return new JsonResponse { Data = await db.Assets.ToListAsync() };
		}

		[HttpGet("Get/{id}")]
		public async Task<ActionResult<JsonResponse>> GetAssets(int? id) {
			if (id == null)
				return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
			var asset = await db.Assets.FindAsync(id);
			if (asset == null)
				return new JsonResponse { Code = -2, Message = $"Asset id={id} not found" };
			return new JsonResponse(asset);
		}

		[HttpPost("Create")]
		public async Task<ActionResult<JsonResponse>> PutAsset(Asset asset) {
			if (asset == null)
				return new JsonResponse { Code = -2, Message = "Parameter asset cannot be null" };
			db.Assets.Add(asset);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Asset Created", Data = asset };
		}

		[HttpPost("Change")]
		public async Task<ActionResult<JsonResponse>> PostAsset(Asset asset) {
			if (asset == null)
				return new JsonResponse { Code = -2, Message = "Parameter asset cannot be null" };
			db.Entry(asset).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Asset Changed", Data = asset };
		}

		[HttpPost("Remove")]
		public async Task<ActionResult<JsonResponse>> DeleteAsset(Asset asset) {
			if (asset == null)
				return new JsonResponse { Code = -2, Message = "Parameter asset cannot be null" };
			db.Assets.Remove(asset);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Asset Removed", Data = asset };
		}

		//private JsonResponse SaveChanges(JsonResponse resp) {
		//	try {
		//		db.SaveChanges();
		//		return resp ?? JsonResponse.Ok;
		//	} catch (Exception ex) {
		//		return new JsonResponse { Message = ex.Message, Error = ex };
		//	}
		//}
	}
}
