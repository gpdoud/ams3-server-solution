

using Ams3.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.ViewModels {

    public class VehiclePrint {
        public string Code { get; set; }
        public string Name { get; set; }
        public string YearMakeModel { get; set; }
        public string LicensePlate { get; set; }
        public string VIN { get; set; }
        public string Description { get; set; }
        public decimal? Cost { get; set; }
        public decimal? ResidualValue { get; set; }
        public DateTime? AcquiredDate { get; set; }
        public DateTime? DisposedDate { get; set; }
        public string Department { get; set; }


        public VehiclePrint(Vehicle vehicle) {
            this.Code = vehicle.Code;
            this.Name = vehicle.Asset.Name;
            this.YearMakeModel = $"{vehicle.Year} {vehicle.Make} {vehicle.Model}";
            this.LicensePlate = vehicle.LicensePlate;
            this.VIN = vehicle.VIN;
            this.Description = vehicle.Asset.Description;
            this.Cost = vehicle.Asset.Cost;
            this.ResidualValue = vehicle.Asset.ResidualValue;
            this.DisposedDate = vehicle.Asset.DisposedDate;
            this.Department = vehicle.Asset.Department.Name;
        }
    }
}