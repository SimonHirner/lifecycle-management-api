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
            return Ok(context.Disposals.Include(d => d.Devices).ToArray());
        }

        /// <summary>
        /// Returns the disposal with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Disposal> GetDisposal(int id)
        {
            var disposal = context.Disposals.Where(c => c.ActivityId == id).Include(a => a.Devices).FirstOrDefault();
            //statement is translated into "SELECT * FROM Disposals WHERE Disposals.ID = id TOP 1" -- only first entry or null

            if (disposal == null) return NotFound();
            return Ok(disposal);
        }

        /// <summary>
        /// Adds a disposal.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Disposal>> AddDisposal([FromBody] Disposal disposal)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Disposal) is valid

                //test if disposal already exists
                if (context.Disposals.Where(c => c.ActivityId == disposal.ActivityId).FirstOrDefault() != null)
                    return Conflict("disposal with id already exists"); //disposal with id already exists, we return a conflict

                context.Disposals.Add(disposal);
                //is translated into "INSERT INTO Disposals (colum names in Disposal class) VALUES (values in disposal object)"

                await context.SaveChangesAsync();

                return Ok(disposal); //we return the disposal
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Disposal
        }

        /// <summary>
        /// Updates a disposal.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Disposal>> UpdateDisposal([FromBody] Disposal disposal)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (disposal.ActivityId == 0)
                {
                    return BadRequest("When updating a disposal, the id must be provided.");
                }

                //we try to find the existing disposal in the database
                var existingDisposal = context.Disposals.Where(c => c.ActivityId == disposal.ActivityId).FirstOrDefault();

                if (existingDisposal != null)
                { //if the disposal exists, existingDisposal is not null

                    //we delete the existing one
                    context.Disposals.Remove(existingDisposal);
                    //Translated into "DELETE FROM Disposals WHERE Disposals.ID = id"

                    //we add the updated one
                    context.Disposals.Add(disposal);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated disposal
                    return Ok(disposal);
                }
                else
                {
                    //disposal doesn't exists
                    return NotFound(); //we couldn't find the disposal in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a disposal with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDisposal([FromRoute] int id)
        {
            var existingDisposal = context.Disposals.Where(c => c.ActivityId == id).FirstOrDefault();
            if (existingDisposal == null)
            {
                return NotFound();
            }
            else
            {
                //delete disposal
                context.Remove(existingDisposal);
                //Translated into "DELETE FROM Disposals WHERE Disposals.ID = existingDisposal.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}