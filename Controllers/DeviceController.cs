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
    /// This endpoint manages all operations for devices.
    /// </summary>
    [Route("api/v1/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private Context context;
        public DeviceController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all devices.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Device[]> GetAllDevices()
        {
            return Ok(context.Devices.ToArray());
        }
    }
}