using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ams3.Models {

	public class Department {

		public int Id { get; set; }
		[Required]
		[StringLength(30)]

		public string Code { get; set; }
		public string Name { get; set; }

		public bool Active { get; set; } = true;
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateUpdated { get; set; } = null;

		public Department() {
		}
	}
}