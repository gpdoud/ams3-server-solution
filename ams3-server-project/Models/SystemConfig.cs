using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.Models {

	public class SystemConfig {
		public int Id { get; set; }
		public string Category { get; set; }
		public string SysKey { get; set; }
		public string SysValue { get; set; }

		public SystemConfig() { }

		public SystemConfig(string syskey, string sysvalue) : this(syskey, sysvalue, null) {
		}

		public SystemConfig(string syskey, string sysvalue, string category) {
			this.Category = category;
			this.SysKey = syskey;
			this.SysValue = sysvalue;
		}
	}
}