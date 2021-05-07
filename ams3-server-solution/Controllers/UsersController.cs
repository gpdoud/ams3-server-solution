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

namespace Ams3.Controllers {

	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : AmsWebApiController {

		//private AmsDbContext db = new AmsDbContext();

		public UsersController(AmsDbContext context) : base(context) { }

		[HttpGet("Login/{username}/{password}")]
		public async Task<ActionResult<JsonResponse>> LoginUser(string username, string password) {
			if (username == null || password == null) return BadRequest();
			var user = await db.Users
								.Include(x => x.Department)
								.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
            if(user == null) {
                return new JsonResponse {
                    Code = -3,
                    Message = "Username/Password combination not found"
                };
            }
			return new JsonResponse(user);
		}

		[HttpGet("List")]
		public async Task<ActionResult<JsonResponse>> GetUsers() {
			return new JsonResponse { Data = await db.Users.Include(x => x.Department).ToListAsync() };
		}

		[HttpGet("Get/{id}")]
		public async Task<ActionResult<JsonResponse>> GetUser(int? id) {
			if (id == null)
				return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
			var user = await db.Users.Include(x => x.Department).SingleOrDefaultAsync(x => x.Id == id);
			if (user == null)
				return new JsonResponse { Code = -2, Message = $"User id={id} not found" };
			return new JsonResponse(user);
		}

		[HttpPost("Create")]
		public async Task<ActionResult<JsonResponse>> AddUser(User user) {
			if (user == null)
				return new JsonResponse { Code = -2, Message = "Parameter user cannot be null" };
			user.DateCreated = DateTime.Now;
			db.Users.Add(user);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "User Created", Data = user };
		}

		[HttpPost]
		[ActionName("Change")]
		public async Task<ActionResult<JsonResponse>> ChangeUser( User user) {
			if (user == null)
				return new JsonResponse { Code = -2, Message = "Parameter user cannot be null" };
			user.DateUpdated = DateTime.Now;
			db.Entry(user).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "User Changed", Data = user };
		}

		[HttpPost]
		[ActionName("Remove")]
		public async Task<ActionResult<JsonResponse>> RemoveUser(User user) {
			if (user == null)
				return new JsonResponse { Code = -2, Message = "Parameter user cannot be null" };
			db.Users.Remove(user);
			await db.SaveChangesAsync();
			return new JsonResponse { Message = "User Removed", Data = user };
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
