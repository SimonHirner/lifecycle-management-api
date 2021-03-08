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
    /// This endpoint manages all operations for disposals.
    /// </summary>
    [Route("api/v1/activities/disposals")]
    [ApiController]
    public class DisposalController : ControllerBase
    {
        private Context context;
        public DisposalController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all disposals.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Disposal[]> GetAllDisposals()
        {
            return Ok(context.Disposals.ToArray());
        }
    }
}