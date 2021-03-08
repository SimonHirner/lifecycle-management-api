using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifecycleManagementAPI.DataObjects;

namespace LifecycleManagementAPI.Controllers
{
    /// <summary>
    /// This endpoint manages all operations for maintenances.
    /// </summary>
    [Route("api/v1/activities/maintenances")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private Context context;
        public MaintenanceController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all maintenances.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Maintenance[]> GetAllMaintenances()
        {
            return Ok(context.Maintenances.ToArray());
        }
    }
}