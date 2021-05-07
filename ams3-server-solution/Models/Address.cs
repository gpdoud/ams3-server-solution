using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.Models {
	public class Address {
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zipcode { get; set; }
		public bool Active { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateUpdated { get; set; }

		public Address(string Code, string Name, string Address1, string Address2, string Address3,
						string City, string State, string Zipcode) {
			this.Code = Code;
			this.Name = Name;
			this.Address1 = Address1;
			this.Address2 = Address2;
			this.Address3 = Address3;
			this.City = City;
			this.State = State;
			this.Zipcode = Zipcode;
		}

		public Address() { }

		public void Copy(Address a) {
			this.Code = a.Code;
			this.Name = a.Name;
			this.Address1 = a.Address1;
			this.Address2 = a.Address2;
			this.Address3 = a.Address3;
			this.City = a.City;
			this.State = a.State;
			this.Zipcode = a.Zipcode;
			this.Active = a.Active;
			this.DateCreated = a.DateCreated;
			this.DateUpdated = a.DateUpdated;
		}
	}
}