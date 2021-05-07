using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ams3.Utility;
using Ams3.Models;

namespace Ams3.Controllers {

    public class AmsWebApiController : ControllerBase {

		protected readonly AmsDbContext db;

		public AmsWebApiController(AmsDbContext context) {
			db = context;
        }

		protected JsonResponse SaveChanges(JsonResponse resp = null) {
			try {
				db.SaveChanges();
				return resp ?? JsonResponse.Ok;
			} catch (Exception ex) {
				return new JsonResponse { Message = ex.Message, Error = ex };
			}
		}
		protected void ClearAssetVirtuals(Vehicle vehicle) {
			vehicle.Asset.Address = null;
			vehicle.Asset.Department = null;
			vehicle.Asset.Category = null;
			vehicle.Asset.User = null;
		}
		protected void ClearAssetVirtuals(Equipment equipment) {
			equipment.Asset.Address = null;
			equipment.Asset.Department = null;
			equipment.Asset.Category = null;
			equipment.Asset.User = null;
		}
		protected void ClearAssetVirtuals(Property property) {
			property.Asset.Address = null;
			property.Asset.Department = null;
			property.Asset.Category = null;
			property.Asset.User = null;
		}
	}
}
