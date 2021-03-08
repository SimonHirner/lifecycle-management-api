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
    /// This endpoint manages all operations for Laptops.
    /// </summary>
    [Route("api/v1/models/laptops")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private Context context;
        public LaptopController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all Laptops.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Laptop[]> GetAllLapTops()
        {
            return Ok(context.Laptops.ToArray());
        }
    }
}