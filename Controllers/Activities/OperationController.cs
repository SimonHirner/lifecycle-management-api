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
    /// This endpoint manages all operations for operations.
    /// </summary>
    [Route("api/v1/activities/operations")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private Context context;
        public OperationController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all operations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Operation[]> GetAllOperations()
        {
            return Ok(context.Operations.Include(o => o.Devices).ToArray());
        }
        
        /// <summary>
        /// Returns the operation with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Operation> GetOperation(int id)
        {
            var operation = context.Operations.Where(c => c.ActivityId == id).Include(a => a.Devices).FirstOrDefault();
            //statement is translated into "SELECT * FROM Operations WHERE Operations.ID = id TOP 1" -- only first entry or null

            if (operation == null) return NotFound();
            return Ok(operation);
        }

        /// <summary>
        /// Adds a operation.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Operation>> AddOperation([FromBody] Operation operation)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Operation) is valid

                //test if operation already exists
                if (context.Operations.Where(c => c.ActivityId == operation.ActivityId).FirstOrDefault() != null)
                    return Conflict("operation with id already exists"); //operation with id already exists, we return a conflict

                context.Operations.Add(operation);
                //is translated into "INSERT INTO Operations (colum names in Operation class) VALUES (values in operation object)"

                await context.SaveChangesAsync();

                return Ok(operation); //we return the operation
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Operation
        }

        /// <summary>
        /// Updates a operation.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Operation>> UpdateOperation([FromBody] Operation operation)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (operation.ActivityId == 0)
                {
                    return BadRequest("When updating a operation, the id must be provided.");
                }

                //we try to find the existing operation in the database
                var existingOperation = context.Operations.Where(c => c.ActivityId == operation.ActivityId).FirstOrDefault();

                if (existingOperation != null)
                { //if the operation exists, existingOperation is not null

                    //we delete the existing one
                    context.Operations.Remove(existingOperation);
                    //Translated into "DELETE FROM Operations WHERE Operations.ID = id"

                    //we add the updated one
                    context.Operations.Add(operation);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated operation
                    return Ok(operation);
                }
                else
                {
                    //operation doesn't exists
                    return NotFound(); //we couldn't find the operation in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a operation with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOperation([FromRoute] int id)
        {
            var existingOperation = context.Operations.Where(c => c.ActivityId == id).FirstOrDefault();
            if (existingOperation == null)
            {
                return NotFound();
            }
            else
            {
                //delete operation
                context.Remove(existingOperation);
                //Translated into "DELETE FROM Operations WHERE Operations.ID = existingOperation.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}