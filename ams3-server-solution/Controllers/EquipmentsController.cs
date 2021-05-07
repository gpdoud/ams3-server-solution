
using Ams3.Controllers;
using Ams3.Models;
using Ams3.Utility;
using Ams3.ViewModels;

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
    public class EquipmentsController : AmsWebApiController {

        //private AmsDbContext db = new AmsDbContext();
        public EquipmentsController(AmsDbContext context) : base(context) { }

        [HttpGet("ListByDepartment")]
        public async Task<ActionResult<JsonResponse>> GetEquipmentsByDepartment(int? id) {
            var equipments = await db.Equipments.Where(e => e.Asset.DepartmentId == id).ToListAsync();
            var equipmentPrints = new List<EquipmentPrint>();
            foreach(var e in equipments) {
                equipmentPrints.Add(new EquipmentPrint(e));
            }
            return new JsonResponse { Data = equipmentPrints };
        }

        [HttpGet("List")]
        public async Task<ActionResult<JsonResponse>> GetEquipment() {
            return new JsonResponse { Data = await db.Equipments.ToListAsync() };
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<JsonResponse>> GetEquipment(int? id) {
            try {
                if(id == null)
                    return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
                var equipment = await db.Equipments.FindAsync(id);
                if(equipment == null)
                    return new JsonResponse { Code = -2, Message = $"Equipment id={id} not found" };
                return new JsonResponse(equipment);
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<JsonResponse>> CreateEquipment(Equipment equipment) {
            try {
                if(equipment == null)
                    return new JsonResponse { Code = -2, Message = "Parameter equipment cannot be null" };
                if(!ModelState.IsValid)
                    return new JsonResponse { Code = -1, Message = "ModelState invalid", Error = ModelState };
                // add the asset first
                // needs all the asset data entered already
                var asset = equipment.Asset;
                db.Assets.Add(asset);
                var recsAffected = await db.SaveChangesAsync(); // so the asset exists for the equipment
                if(recsAffected != 1)
                    return new JsonResponse { Code = -2, Message = "Create asset failed while attempting to add equipment" };
                equipment.AssetId = asset.Id; // this gets the generated PK
                equipment.DateCreated = DateTime.Now;
                db.Equipments.Add(equipment);
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Equipment Created", Data = equipment };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost("Change")]
        public async Task<ActionResult<JsonResponse>> ChangeEquipment(Equipment equipment) {
            try {
                if(equipment == null)
                    return new JsonResponse { Code = -2, Message = "Parameter equipment cannot be null" };
                // issue #11
                // If the addressId in the asset is set to null (clears the address dropdown)
                // set the Asset instance to null also.
                //if (equipment.Asset.AddressId == null)
                //	equipment.Asset.Address = null;
                ClearAssetVirtuals(equipment);
                equipment.DateUpdated = DateTime.Now;
                db.Entry(equipment.Asset).State = EntityState.Modified;
                db.Entry(equipment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Equipment Changed", Data = equipment };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost("Remove")]
        public async Task<ActionResult<JsonResponse>> RemoveEquipment(Equipment equipment) {
            try {
                if(equipment == null)
                    return new JsonResponse { Code = -2, Message = "Parameter equipment cannot be null" };
                db.Equipments.Remove(equipment);
                // the related equipment record will be deleted also because
                // of cascading delete
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Equipment Removed", Data = equipment };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
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
