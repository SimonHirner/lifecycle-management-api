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

        /*
        /// <summary>
        /// Adds a activity.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Activity>> AddActivity([FromBody] Activity activity)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Activity) is valid

                //test if activity already exists
                if (context.Activities.Where(c => c.ActivityId == activity.ActivityId).FirstOrDefault() != null)
                    return Conflict("activity with id already exists"); //activity with id already exists, we return a conflict

                context.Activities.Add(activity);
                //is translated into "INSERT INTO Activities (colum names in Activity class) VALUES (values in activity object)"

                await context.SaveChangesAsync();

                return Ok(activity); //we return the activity
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Activity
        }

        /// <summary>
        /// Updates a activity.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Activity>> UpdateActivity([FromBody] Activity activity)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (activity.ActivityId == 0)
                {
                    return BadRequest("When updating a activity, the id must be provided.");
                }

                //we try to find the existing activity in the database
                var existingActivity = context.Activities.Where(c => c.ActivityId == activity.ActivityId).FirstOrDefault();

                if (existingActivity != null)
                { //if the activity exists, existingActivity is not null

                    //we delete the existing one
                    context.Activities.Remove(existingActivity);
                    //Translated into "DELETE FROM Activities WHERE Activities.ID = id"

                    //we add the updated one
                    context.Activities.Add(activity);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated activity
                    return Ok(activity);
                }
                else
                {
                    //activity doesn't exists
                    return NotFound(); //we couldn't find the activity in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a activity with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteActivity([FromRoute] int id)
        {
            var existingActivity = context.Activities.Where(c => c.ActivityId == id).FirstOrDefault();
            if (existingActivity == null)
            {
                return NotFound();
            }
            else
            {
                //delete activity
                context.Remove(existingActivity);
                //Translated into "DELETE FROM Activities WHERE Activities.ID = existingActivity.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
        */
    }
}