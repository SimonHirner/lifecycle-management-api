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
            return Ok(context.Workstations.Include(w => w.Devices).ToArray());
        }
                
        /// <summary>
        /// Returns the workstation with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Workstation> GetWorkstation(int id)
        {
            var workstation = context.Workstations.Where(c => c.ModelId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Workstations WHERE Workstations.ID = id TOP 1" -- only first entry or null

            if (workstation == null) return NotFound();
            return Ok(workstation);
        }

        /// <summary>
        /// Adds a workstation.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Workstation>> AddWorkstation([FromBody] Workstation workstation)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Workstation) is valid

                //test if workstation already exists
                if (context.Workstations.Where(c => c.ModelId == workstation.ModelId).FirstOrDefault() != null)
                    return Conflict("workstation with id already exists"); //workstation with id already exists, we return a conflict

                context.Workstations.Add(workstation);
                //is translated into "INSERT INTO Workstations (colum names in Workstation class) VALUES (values in workstation object)"

                await context.SaveChangesAsync();

                return Ok(workstation); //we return the workstation
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Workstation
        }

        /// <summary>
        /// Updates a workstation.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Workstation>> UpdateWorkstation([FromBody] Workstation workstation)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (workstation.ModelId == 0)
                {
                    return BadRequest("When updating a workstation, the id must be provided.");
                }

                //we try to find the existing workstation in the database
                var existingWorkstation = context.Workstations.Where(c => c.ModelId == workstation.ModelId).FirstOrDefault();

                if (existingWorkstation != null)
                { //if the workstation exists, existingWorkstation is not null

                    //we delete the existing one
                    context.Workstations.Remove(existingWorkstation);
                    //Translated into "DELETE FROM Workstations WHERE Workstations.ID = id"

                    //we add the updated one
                    context.Workstations.Add(workstation);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated workstation
                    return Ok(workstation);
                }
                else
                {
                    //workstation doesn't exists
                    return NotFound(); //we couldn't find the workstation in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a workstation with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteWorkstation([FromRoute] int id)
        {
            var existingWorkstation = context.Workstations.Where(c => c.ModelId == id).FirstOrDefault();
            if (existingWorkstation == null)
            {
                return NotFound();
            }
            else
            {
                //delete workstation
                context.Remove(existingWorkstation);
                //Translated into "DELETE FROM Workstations WHERE Workstations.ID = existingWorkstation.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}