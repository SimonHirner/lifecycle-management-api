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
            return Ok(context.Maintenances.Include(m => m.Devices).ToArray());
        }
        
        /// <summary>
        /// Returns the maintenance with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Maintenance> GetMaintenance(int id)
        {
            var maintenance = context.Maintenances.Where(c => c.ActivityId == id).Include(a => a.Devices).FirstOrDefault();
            //statement is translated into "SELECT * FROM Maintenances WHERE Maintenances.ID = id TOP 1" -- only first entry or null

            if (maintenance == null) return NotFound();
            return Ok(maintenance);
        }

        /// <summary>
        /// Adds a maintenance.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Maintenance>> AddMaintenance([FromBody] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Maintenance) is valid

                //test if maintenance already exists
                if (context.Maintenances.Where(c => c.ActivityId == maintenance.ActivityId).FirstOrDefault() != null)
                    return Conflict("maintenance with id already exists"); //maintenance with id already exists, we return a conflict

                context.Maintenances.Add(maintenance);
                //is translated into "INSERT INTO Maintenances (colum names in Maintenance class) VALUES (values in maintenance object)"

                await context.SaveChangesAsync();

                return Ok(maintenance); //we return the maintenance
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Maintenance
        }

        /// <summary>
        /// Updates a maintenance.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Maintenance>> UpdateMaintenance([FromBody] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (maintenance.ActivityId == 0)
                {
                    return BadRequest("When updating a maintenance, the id must be provided.");
                }

                //we try to find the existing maintenance in the database
                var existingMaintenance = context.Maintenances.Where(c => c.ActivityId == maintenance.ActivityId).FirstOrDefault();

                if (existingMaintenance != null)
                { //if the maintenance exists, existingMaintenance is not null

                    //we delete the existing one
                    context.Maintenances.Remove(existingMaintenance);
                    //Translated into "DELETE FROM Maintenances WHERE Maintenances.ID = id"

                    //we add the updated one
                    context.Maintenances.Add(maintenance);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated maintenance
                    return Ok(maintenance);
                }
                else
                {
                    //maintenance doesn't exists
                    return NotFound(); //we couldn't find the maintenance in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a maintenance with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMaintenance([FromRoute] int id)
        {
            var existingMaintenance = context.Maintenances.Where(c => c.ActivityId == id).FirstOrDefault();
            if (existingMaintenance == null)
            {
                return NotFound();
            }
            else
            {
                //delete maintenance
                context.Remove(existingMaintenance);
                //Translated into "DELETE FROM Maintenances WHERE Maintenances.ID = existingMaintenance.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}