using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifecycleManagementAPI.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagementAPI.Controllers
{
    /// <summary>
    /// This endpoint manages all operations for activities.
    /// </summary>
    [Route("api/v1/activities")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private Context context;
        public ActivityController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all activities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Activity[]> GetAllActivities()
        {
            return Ok(context.Activities.Include(a => a.Devices).ToArray());
        }
        
        /// <summary>
        /// Returns the activity with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Activity> GetActivity(int id)
        {
            var activity = context.Activities.Where(c => c.ActivityId == id).Include(a => a.Devices).FirstOrDefault();
            //statement is translated into "SELECT * FROM Activities WHERE Activities.ID = id TOP 1" -- only first entry or null

            if (activity == null) return NotFound();
            return Ok(activity);
        }
    }
}