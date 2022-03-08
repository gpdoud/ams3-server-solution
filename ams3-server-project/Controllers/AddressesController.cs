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
	public class AddressesController : AmsWebApiController {

		public AddressesController(AmsDbContext context) : base(context) { }

		[HttpGet("List")]
		public async Task<ActionResult<JsonResponse>> GetAddresss() {
			return new JsonResponse { Data = await db.Addresses.ToListAsync() };
		}

		[HttpGet("Get/{id}")]
		public async Task<ActionResult<JsonResponse>> GetAddress(int? id) {
			if (id == null)
				return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
			var address = await db.Addresses.FindAsync(id);
			if (address == null)
				return new JsonResponse { Code = -2, Message = $"Address id={id} not found" };
			return new JsonResponse(address);
		}

		[HttpPost("Create")]
		public async Task<ActionResult<JsonResponse>> AddAddress(Address address) {
			if (address == null)
				return new JsonResponse { Code = -2, Message = "Parameter address cannot be null" };
			address.DateCreated = DateTime.Now;
			db.Addresses.Add(address);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Address Created", Data = address };
		}

		[HttpPost("Change")]
		public async Task<ActionResult<JsonResponse>> ChangeAddress(Address address) {
			if (address == null)
				return new JsonResponse { Code = -2, Message = "Parameter address cannot be null" };
			address.DateUpdated = DateTime.Now;
			db.Entry(address).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Address Changed", Data = address };
		}

		[HttpPost("Remove")]
		public async Task<ActionResult<JsonResponse>> RemoveAddress(Address address) {
			if (address == null)
				return new JsonResponse { Code = -2, Message = "Parameter address cannot be null" };
			db.Addresses.Remove(address);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "Address Removed", Data = address };
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
