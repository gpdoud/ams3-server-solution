

using Ams3.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.ViewModels {

    public class EquipmentPrint {
        public string Code { get; set; }
        public string Description { get; set; }
        public string YearMakeModel { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public decimal? Cost { get; set; }
        public decimal? ResidualValue { get; set; }
        public DateTime? AcquiredDate { get; set; }
        public DateTime? DisposedDate { get; set; }
        public string Department { get; set; }

        public EquipmentPrint(Equipment equipment) {
            this.Code = equipment.Code;
            this.Description = equipment.Description;
            this.YearMakeModel = $"{equipment.Year} {equipment.Make} {equipment.Model}";
            this.SerialNumber = equipment.SerialNumber;
            this.Name = equipment.Asset.Name;
            this.Cost = equipment.Asset.Cost;
            this.ResidualValue = equipment.Asset.ResidualValue;
            this.AcquiredDate = equipment.Asset.AcquiredDate;
            this.DisposedDate = equipment.Asset.DisposedDate;
            this.Department = equipment.Asset.Department.Name;
        }
    }
}