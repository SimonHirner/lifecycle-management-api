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
            return Ok(context.Laptops.Include(l => l.Devices).ToArray());
        }
        
        /// <summary>
        /// Returns the laptop with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Laptop> GetLaptop(int id)
        {
            var laptop = context.Laptops.Where(c => c.ModelId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Laptops WHERE Laptops.ID = id TOP 1" -- only first entry or null

            if (laptop == null) return NotFound();
            return Ok(laptop);
        }

        /// <summary>
        /// Adds a laptop.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Laptop>> AddLaptop([FromBody] Laptop laptop)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Laptop) is valid

                //test if laptop already exists
                if (context.Laptops.Where(c => c.ModelId == laptop.ModelId).FirstOrDefault() != null)
                    return Conflict("laptop with id already exists"); //laptop with id already exists, we return a conflict

                context.Laptops.Add(laptop);
                //is translated into "INSERT INTO Laptops (colum names in Laptop class) VALUES (values in laptop object)"

                await context.SaveChangesAsync();

                return Ok(laptop); //we return the laptop
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Laptop
        }

        /// <summary>
        /// Updates a laptop.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Laptop>> UpdateLaptop([FromBody] Laptop laptop)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (laptop.ModelId == 0)
                {
                    return BadRequest("When updating a laptop, the id must be provided.");
                }

                //we try to find the existing laptop in the database
                var existingLaptop = context.Laptops.Where(c => c.ModelId == laptop.ModelId).FirstOrDefault();

                if (existingLaptop != null)
                { //if the laptop exists, existingLaptop is not null

                    //we delete the existing one
                    context.Laptops.Remove(existingLaptop);
                    //Translated into "DELETE FROM Laptops WHERE Laptops.ID = id"

                    //we add the updated one
                    context.Laptops.Add(laptop);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated laptop
                    return Ok(laptop);
                }
                else
                {
                    //laptop doesn't exists
                    return NotFound(); //we couldn't find the laptop in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a laptop with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteLaptop([FromRoute] int id)
        {
            var existingLaptop = context.Laptops.Where(c => c.ModelId == id).FirstOrDefault();
            if (existingLaptop == null)
            {
                return NotFound();
            }
            else
            {
                //delete laptop
                context.Remove(existingLaptop);
                //Translated into "DELETE FROM Laptops WHERE Laptops.ID = existingLaptop.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}