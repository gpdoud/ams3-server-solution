
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
    public class PropertiesController : AmsWebApiController {

        public PropertiesController(AmsDbContext context) : base(context) { }

        [HttpGet("List")]
        public async Task<ActionResult<JsonResponse>> GetProperty() {
            return new JsonResponse { Data = await db.Properties.ToListAsync() };
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<JsonResponse>> GetProperty(int? id) {
            try {
                if(id == null)
                    return new JsonResponse { Message = "Parameter id cannot be null" };
                var property = await db.Properties.Include(x => x.Asset).SingleOrDefaultAsync(x => x.Id == id);
                if(property == null)
                    return new JsonResponse { Message = $"Property id={id} not found" };
                return new JsonResponse(property);
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<JsonResponse>> CreateProperty( Property property) {
            try {
                if(property == null)
                    return new JsonResponse { Message = "Parameter property cannot be null" };
                if(!ModelState.IsValid)
                    return new JsonResponse { Message = "ModelState invalid", Error = ModelState };
                // add the asset first
                // needs all the asset data entered already
                var asset = property.Asset;
                db.Assets.Add(property.Asset);
                await db.SaveChangesAsync(); // so the asset exists for the property
                property.AssetId = asset.Id; // this gets the generated PK
                property.DateCreated = DateTime.Now;
                db.Properties.Add(property);
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Property Created", Data = property };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost("Change")]
        public async Task<ActionResult<JsonResponse>> ChangeProperty( Property property) {
            try {
                if(property == null)
                    return new JsonResponse { Message = "Parameter property cannot be null" };
                // issue #11
                // If the addressId in the asset is set to null (clears the address dropdown)
                // set the Asset instance to null also.
                //if (property.Asset.AddressId == null)
                //	property.Asset.Address = null;
                ClearAssetVirtuals(property);

                if(!ModelState.IsValid)
                    return new JsonResponse { Message = "ModelState invalid", Error = ModelState };
                property.DateUpdated = DateTime.Now;
                db.Entry(property.Asset).State = EntityState.Modified;
                db.Entry(property).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Property Changed", Data = property };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost("Remove")]
        public async Task<ActionResult<JsonResponse>> RemoveProperty( Property property) {
            try {
                if(property == null)
                    return new JsonResponse { Message = "Parameter property cannot be null" };
                db.Properties.Remove(property);
                // the related property record will be deleted also because
                // of cascading delete
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Property Removed", Data = property };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }
    }
}
