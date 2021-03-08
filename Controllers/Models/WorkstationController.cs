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
    /// This endpoint manages all operations for workstations.
    /// </summary>
    [Route("api/v1/models/workstations")]
    [ApiController]
    public class WorkstationController : ControllerBase
    {
        private Context context;
        public WorkstationController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all workstations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Workstation[]> GetAllWorkstations()
        {
            return Ok(context.Workstations.ToArray());
        }
    }
}