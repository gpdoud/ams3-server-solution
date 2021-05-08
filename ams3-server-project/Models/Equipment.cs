using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ams3.Models {
	/// <summary>
	/// Equipment are things like shovels, rakes, etc.
	/// </summary>
	public class Equipment {
		public int Id { get; set; }
		public string Code { get; set; }
		public int AssetId { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int? Year { get; set; }
		public string Description { get; set; }
		[StringLength(50)]
		public string SerialNumber { get; set; }
		public bool Active { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateUpdated { get; set; }

		public virtual Asset Asset { get; set; }

		public Equipment() { }

		public Equipment(int AssetId, Asset Asset, string Description, string SerialNumber) {
			this.AssetId = AssetId;
			this.Asset = Asset;
			this.Description = Description;
			this.SerialNumber = SerialNumber;
			this.Active = true;
			this.DateCreated = DateTime.Now;
		}

		public void Copy(Equipment e) {
			this.AssetId = e.AssetId;
			this.Asset = e.Asset;
			this.Code = e.Code;
			this.Year = e.Year;
			this.Make = e.Make;
			this.Model = e.Model;
			this.Description = e.Description;
			this.SerialNumber = e.SerialNumber;
			this.Active = e.Active;
			this.DateCreated = e.DateCreated;
			this.DateUpdated = e.DateUpdated;
		}
	}
}