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
    public class DepartmentsController : AmsWebApiController {
        //private AmsDbContext db = new AmsDbContext();

        public DepartmentsController(AmsDbContext context) : base(context) { }

        [HttpGet("List")]
        public async Task<ActionResult<JsonResponse>> GetDepartments() {
            return new JsonResponse { Data = await db.Departments.ToListAsync() };
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<JsonResponse>> GetDepartment(int? id) {
            if(id == null)
                return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
            var department = await db.Departments.FindAsync(id);
            if(department == null)
                return new JsonResponse { Code = -2, Message = $"Department id={id} not found" };
            return new JsonResponse(department);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<JsonResponse>> AddDepartment(Department department) {
            if(department == null)
                return new JsonResponse { Code = -2, Message = "Parameter department cannot be null" };
            department.DateCreated = DateTime.Now;
            db.Departments.Add(department);
            await db.SaveChangesAsync();
            return new JsonResponse { Message = "Department Created", Data = department };
        }

        [HttpPost("Change")]
        public async Task<ActionResult<JsonResponse>> ChangeDepartment(Department department) {
            if(department == null)
                return new JsonResponse { Code = -2, Message = "Parameter department cannot be null" };
            department.DateUpdated = DateTime.Now;
            db.Entry(department).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return new JsonResponse { Message = "Department Changed", Data = department };
        }

        [HttpPost("Remove")]
        public async Task<ActionResult<JsonResponse>> RemoveDepartment(Department department) {
            if(department == null)
                return new JsonResponse { Code = -2, Message = "Parameter department cannot be null" };
            db.Departments.Remove(department);
            await db.SaveChangesAsync();
            return new JsonResponse { Message = "Department Removed", Data = department };
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
