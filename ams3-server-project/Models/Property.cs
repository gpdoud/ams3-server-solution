using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.Models {
	public class Property {
		public int Id { get; set; }

		public string Code { get; set; }
		public string Description { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zipcode { get; set; }

		public int AssetId { get; set; }

		public bool Active { get; set; } = true;
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateUpdated { get; set; } = null;

		public virtual Asset Asset { get; set; }

		public Property() { }
	}
}