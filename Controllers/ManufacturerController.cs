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
    /// This endpoint manages all operations for manufacturers.
    /// </summary>
    [Route("api/v1/manufacturers")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private Context context;
        public ManufacturerController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all manufacturers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Manufacturer[]> GetAllManufacturers()
        {
            return Ok(context.Manufacturers.ToArray());
        }
    }
}