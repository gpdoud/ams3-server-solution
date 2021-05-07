using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.Models {

	public class Vehicle {

		public int Id { get; set; }
		public string Code { get; set; }
		public int AssetId { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int? Year { get; set; }
		public string LicensePlate { get; set; }
		public string VIN { get; set; }
        public string TitleNo { get; set; }
		public bool Active { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateUpdated { get; set; }

		public virtual Asset Asset { get; set; }

		public void Copy(Vehicle vehicle) {
			this.Code = vehicle.Code;
			this.AssetId = vehicle.AssetId;
			this.Make = vehicle.Make;
			this.Model = vehicle.Model;
			this.Year = vehicle.Year;
			this.LicensePlate = vehicle.LicensePlate;
			this.VIN = vehicle.VIN;
            this.TitleNo = vehicle.TitleNo;
			this.Active = vehicle.Active;
			this.DateCreated = vehicle.DateCreated;
			this.DateUpdated = DateTime.Now;
		}

		public Vehicle() {
		}
	}
}