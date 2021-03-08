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
            return Ok(context.Manufacturers.Include(m => m.Models).ToArray());
        }
        
        /// <summary>
        /// Returns the manufacturer with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Manufacturer> GetManufacturer(int id)
        {
            var manufacturer = context.Manufacturers.Where(c => c.ManufacturerId == id).Include(a => a.Models).FirstOrDefault();
            //statement is translated into "SELECT * FROM Manufacturers WHERE Manufacturers.ID = id TOP 1" -- only first entry or null

            if (manufacturer == null) return NotFound();
            return Ok(manufacturer);
        }

        /// <summary>
        /// Adds a manufacturer.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Manufacturer>> AddManufacturer([FromBody] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Manufacturer) is valid

                //test if manufacturer already exists
                if (context.Manufacturers.Where(c => c.ManufacturerId == manufacturer.ManufacturerId).FirstOrDefault() != null)
                    return Conflict("manufacturer with id already exists"); //manufacturer with id already exists, we return a conflict

                context.Manufacturers.Add(manufacturer);
                //is translated into "INSERT INTO Manufacturers (colum names in Manufacturer class) VALUES (values in manufacturer object)"

                await context.SaveChangesAsync();

                return Ok(manufacturer); //we return the manufacturer
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Manufacturer
        }

        /// <summary>
        /// Updates a manufacturer.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Manufacturer>> UpdateManufacturer([FromBody] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (manufacturer.ManufacturerId == 0)
                {
                    return BadRequest("When updating a manufacturer, the id must be provided.");
                }

                //we try to find the existing manufacturer in the database
                var existingManufacturer = context.Manufacturers.Where(c => c.ManufacturerId == manufacturer.ManufacturerId).FirstOrDefault();

                if (existingManufacturer != null)
                { //if the manufacturer exists, existingManufacturer is not null

                    //we delete the existing one
                    context.Manufacturers.Remove(existingManufacturer);
                    //Translated into "DELETE FROM Manufacturers WHERE Manufacturers.ID = id"

                    //we add the updated one
                    context.Manufacturers.Add(manufacturer);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated manufacturer
                    return Ok(manufacturer);
                }
                else
                {
                    //manufacturer doesn't exists
                    return NotFound(); //we couldn't find the manufacturer in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a manufacturer with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteManufacturer([FromRoute] int id)
        {
            var existingManufacturer = context.Manufacturers.Where(c => c.ManufacturerId == id).FirstOrDefault();
            if (existingManufacturer == null)
            {
                return NotFound();
            }
            else
            {
                //delete manufacturer
                context.Remove(existingManufacturer);
                //Translated into "DELETE FROM Manufacturers WHERE Manufacturers.ID = existingManufacturer.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}