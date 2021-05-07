using Ams3.Utility;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.Models {

	public class Logger {

		public int Id { get; set; }
		public DateTime Timestamp { get; set; }
		public string Application { get; set; }
		public string Classname { get; set; }
		public string Method { get; set; }
		public string LogLevel { get; set; }
		public string Message { get; set; }

		public Logger(string message) : this(null, null, null, LogLevelVals.Info, message) {
		}
		public Logger(string logLevel, string message) : this(null, null, null, logLevel, message) {
		}
		public Logger(string application, string classname, string method, string logLevel, string message) {
			this.Timestamp = DateTime.Now;
			this.Application = application;
			this.Classname = classname;
			this.Method = method;
			this.LogLevel = logLevel;
			this.Message = message;
		}
	}
}