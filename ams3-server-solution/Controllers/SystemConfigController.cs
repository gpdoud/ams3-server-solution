
using Ams3.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ams3.Controllers {

	[Route("api/[controller]")]
	[ApiController]
	public class SystemConfigController : ControllerBase {

		private readonly AmsDbContext db;

		public SystemConfigController(AmsDbContext context) {
			db = context;
		}

		[HttpGet("List")]
		public async Task<ActionResult<IEnumerable<SystemConfig>>> GetAll() {
			return await db.SystemConfig.ToListAsync();
		}

		[HttpGet("GetKey/{syskey}")]
		public async Task<ActionResult<SystemConfig>> GetValueByKey(string syskey) {
			if (syskey == null) return null;
			var systemConfig = await getByKey(syskey);
			if (systemConfig == null) return null;
			return systemConfig;
		}

		[HttpGet("SetKey/{syskey}/{sysvalue}/{category}")]
		public async Task<ActionResult<bool>> SetValueByKey(string syskey, string sysvalue, string category = null) {
			if (syskey == null) return false;
			SystemConfig syscfg = new SystemConfig(syskey, sysvalue, category);
			var syscfg2 = await getByKey(syskey);
			if(syscfg2 == null) { // doesn't exist; add
				db.SystemConfig.Add(syscfg);
			} else { // exists; change
				syscfg2.Category = syscfg.Category;
				syscfg2.SysKey = syscfg.SysKey;
				syscfg2.SysValue = syscfg.SysValue;
			}
			await db.SaveChangesAsync();
			return true;
		}

		private async Task<SystemConfig> getByKey(string syskey) {
			if (syskey == null) return null;
			var systemConfig = await db.SystemConfig.SingleOrDefaultAsync(sc => sc.SysKey == syskey);
			return systemConfig;
		}
	}
}
