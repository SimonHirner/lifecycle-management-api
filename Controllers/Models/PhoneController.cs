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
    /// This endpoint manages all operations for phones.
    /// </summary>
    [Route("api/v1/models/phones")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private Context context;
        public PhoneController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all phones.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Phone[]> GetAllPhones()
        {
            return Ok(context.Phones.Include(p => p.Devices).ToArray());
        }
                
        /// <summary>
        /// Returns the phone with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Phone> GetPhone(int id)
        {
            var phone = context.Phones.Where(c => c.ModelId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Phones WHERE Phones.ID = id TOP 1" -- only first entry or null

            if (phone == null) return NotFound();
            return Ok(phone);
        }

        /// <summary>
        /// Adds a phone.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Phone>> AddPhone([FromBody] Phone phone)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Phone) is valid

                //test if phone already exists
                if (context.Phones.Where(c => c.ModelId == phone.ModelId).FirstOrDefault() != null)
                    return Conflict("phone with id already exists"); //phone with id already exists, we return a conflict

                context.Phones.Add(phone);
                //is translated into "INSERT INTO Phones (colum names in Phone class) VALUES (values in phone object)"

                await context.SaveChangesAsync();

                return Ok(phone); //we return the phone
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Phone
        }

        /// <summary>
        /// Updates a phone.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Phone>> UpdatePhone([FromBody] Phone phone)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (phone.ModelId == 0)
                {
                    return BadRequest("When updating a phone, the id must be provided.");
                }

                //we try to find the existing phone in the database
                var existingPhone = context.Phones.Where(c => c.ModelId == phone.ModelId).FirstOrDefault();

                if (existingPhone != null)
                { //if the phone exists, existingPhone is not null

                    //we delete the existing one
                    context.Phones.Remove(existingPhone);
                    //Translated into "DELETE FROM Phones WHERE Phones.ID = id"

                    //we add the updated one
                    context.Phones.Add(phone);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated phone
                    return Ok(phone);
                }
                else
                {
                    //phone doesn't exists
                    return NotFound(); //we couldn't find the phone in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a phone with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePhone([FromRoute] int id)
        {
            var existingPhone = context.Phones.Where(c => c.ModelId == id).FirstOrDefault();
            if (existingPhone == null)
            {
                return NotFound();
            }
            else
            {
                //delete phone
                context.Remove(existingPhone);
                //Translated into "DELETE FROM Phones WHERE Phones.ID = existingPhone.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}