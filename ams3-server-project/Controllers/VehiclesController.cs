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

    /// <summary>
    /// The Vehicle class is an specific Asset although the Asset table
    /// is separate from the Vehicle table. Therefore, when a Vehicle is
    /// added, changed, or deleted, both the Asset and the Vehicle must 
    /// be added, changed, or deleted together.
    /// 
    /// The Vehicle to Asset is a one-to-one relationship.
    /// </summary>
	[Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : AmsWebApiController {

        //private AmsDbContext db = new AmsDbContext();
        enum CtrlMethod { Create, Edit };

        public VehiclesController(AmsDbContext context) : base(context) { }

        [HttpGet("ListByDepartment")]
        public async Task<ActionResult<JsonResponse>> GetVehiclesByDepartment(int? id) {
            var vehicles = await db.Vehicles.Where(v => v.Asset.DepartmentId == id).ToListAsync();
            var vehiclePrints = new List<VehiclePrint>();
            foreach(var v in vehicles) {
                vehiclePrints.Add(new VehiclePrint(v));
            }
            return new JsonResponse { Data = vehiclePrints };
        }

        [HttpGet("List")]
        public async Task<ActionResult<JsonResponse>> GetVehicles() {
            return new JsonResponse { 
                Data = await db.Vehicles
                                .Include(x => x.Asset)
                                .ToListAsync() 
            };
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<JsonResponse>> GetVehicle(int? id) {
            try {
                if(id == null)
                    return new JsonResponse { Code = -2, Message = "Parameter id cannot be null" };
                var vehicle = await db.Vehicles
                                        .Include(x => x.Asset).ThenInclude(x => x.User)
                                        .Include(x => x.Asset).ThenInclude(x => x.Department)
                                        .Include(x => x.Asset).ThenInclude(x => x.Category)
                                        .Include(x => x.Asset).ThenInclude(x => x.Address)
                                        .SingleOrDefaultAsync(v => v.Id == id);
                if(vehicle == null)
                    return new JsonResponse { Code = -2, Message = $"Vehicle id={id} not found" };
                return new JsonResponse(vehicle); // may be null 
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }
        /// <summary>
        /// Check all fields that can be null, but, if not, must be unique
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        private bool AllFieldsAreNullOrUnique(Vehicle vehicle, CtrlMethod method) {
            return IsLicensePlateUniqueIfNotNullOrBlank(vehicle, method)
                /* && IsVinUniqueIfNotNull(vehicle, method) */;
        }
        /// <summary>
        /// This routine checks whether the licenseplate field that will be added or
        /// changed is unique. It is ok if it is null, but if there is a value, 
        /// it cannot already exist on another vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>True if the LicensePlace is null or does not already exist; 
        /// otherwise, it returns false.</returns>
        private bool IsLicensePlateUniqueIfNotNullOrBlank(Vehicle vehicle, CtrlMethod method) {
            // 
            if(vehicle.LicensePlate == null || vehicle.LicensePlate == string.Empty)
                return true;
            var db1 = new AmsDbContext();
            var vehicleDb = db1.Vehicles.SingleOrDefault(v => v.LicensePlate == vehicle.LicensePlate);
            // if creating a new vehicle and we find another vehicle with the same
            // LicensePlate, must cause the create to fail
            if(method == CtrlMethod.Create && vehicleDb != null)
                return false;
            // if editing an existing vehicle and we find a vehicle with the same
            // LicensePlate but it has a different primary key, must cause the 
            // edit to fail
            if(method == CtrlMethod.Edit)
                if(vehicleDb != null && vehicleDb.Id != vehicle.Id)
                    return false;
            // otherwise, the create/edit is ok
            return true;
        }
        /// <summary>
        /// This routine checks whether the VIN field that will be added or
        /// changed is unique. It is ok if it is null, but if there is a value, 
        /// it cannot already exist on another vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>True if the VIN is null or does not already exist; 
        /// otherwise, it returns false.</returns>
        private bool IsVinUniqueIfNotNull(Vehicle vehicle, CtrlMethod method) {
            if(vehicle.VIN == null)
                return true;
            var vehicleDb = db.Vehicles.SingleOrDefault(v => v.VIN == vehicle.VIN);
            // if creating a new vehicle and we find another vehicle with the same
            // VIN, must cause the create to fail
            if(method == CtrlMethod.Create && vehicleDb != null)
                return false;
            // if editing an existing vehicle and we find a vehicle with the same
            // VIN but it has a different primary key, must cause the 
            // edit to fail
            if(method == CtrlMethod.Edit)
                if(vehicleDb != null && vehicleDb.Id != vehicle.Id)
                    return false;
            // otherwise, the create/edit is ok
            return true;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<JsonResponse>> PutVehicle(Vehicle vehicle) {
            try {
                if(vehicle == null)
                    return new JsonResponse { Code = -2, Message = "Parameter vehicle cannot be null" };
                if(!AllFieldsAreNullOrUnique(vehicle, CtrlMethod.Create)) {
                    return new JsonResponse { Code = -2, Message = "ERROR: VIN or LicensePlate is not unique", Error = vehicle };
                }
                // add the asset first
                var asset = vehicle.Asset;
                db.Assets.Add(asset);
                await db.SaveChangesAsync(); // so the asset exists for the vehicle
                vehicle.AssetId = asset.Id; // this gets the generated PK
                db.Vehicles.Add(vehicle);
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Vehicle Created", Data = vehicle };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost]
        [ActionName("Change")]
        public async Task<ActionResult<JsonResponse>> PostVehicle(Vehicle vehicle) {
            try {
                if(vehicle == null)
                    return new JsonResponse { Code = -2, Message = "Parameter vehicle cannot be null" };
                //vehicle.Asset.Address = null;
                //vehicle.Asset.Department = null;
                //vehicle.Asset.Category = null;
                //vehicle.Asset.User = null;
                if(!AllFieldsAreNullOrUnique(vehicle, CtrlMethod.Edit)) {
                    return new JsonResponse { Code = -2, Message = "ERROR: VIN or LicensePlate is not unique", Error = vehicle };
                }
                ClearAssetVirtuals(vehicle);
                if(!ModelState.IsValid)
                    return new JsonResponse { Code = -1, Message = "ModelState invalid", Error = ModelState };
                vehicle.DateUpdated = DateTime.Now;
                db.Entry(vehicle.Asset).State = EntityState.Modified;
                db.Entry(vehicle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Vehicle Changed", Data = vehicle };
            } catch(Exception ex) {
                var jr = JsonResponse.CreateJsonResponseExceptionInstance(-999, $"EXCEPTION: {ex.Message}", ex);
                return jr;
            }
        }

        [HttpPost]
        [ActionName("Remove")]
        public async Task<ActionResult<JsonResponse>> DeleteVehicle(Vehicle vehicle) {
            try {
                if(vehicle == null)
                    return new JsonResponse { Code = -2, Message = "Parameter vehicle cannot be null" };
                var v = await db.Vehicles.FindAsync(vehicle.Id);
                var a = await db.Assets.FindAsync(vehicle.Asset.Id);
                db.Vehicles.Remove(v);
                db.Assets.Remove(a);

                //db.Entry(vehicle.Asset).State = System.Data.Entity.EntityState.Deleted;
                await db.SaveChangesAsync();
                return new JsonResponse { Message = "Vehicle Removed", Data = vehicle };
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
