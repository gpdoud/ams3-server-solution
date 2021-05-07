using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Ams3.Utility {
	public class JsonResponse {

		public static JsonResponse Ok = new JsonResponse("Ok");

		public int Code { get; set; } = 0;
        public string Message { get; set; } = "Success";
		public object Data { get; set; } = null;
		public object Error { get; set; } = null;
        public string MethodName { get; set; } = null;
        public string FormattedMessage { get { return $"{MethodName}{Message}"; } }

        public static JsonResponse CreateJsonResponseExceptionInstance(int Code = -999, string Message = "EXCEPTION:", object Error = null) {
            var jr = new JsonResponse();
            jr.Code = Code;
            jr.Message = Message;
            jr.Error = Error;
            var methodBase = new StackTrace(1).GetFrame(0).GetMethod();
            var methodName = methodBase.Name;
            var className = methodBase.DeclaringType.Name;
            jr.MethodName = $"<{className}.{methodName}>";
            return jr;
        }

        public JsonResponse() {
        }

		public JsonResponse(int Code, string Message) {
			this.Code = Code;
			this.Message = Message;
		}
		public JsonResponse(string Message) {
			this.Message = Message;
		}
		public JsonResponse(object Data) {
			this.Data = Data;
		}
	}
}