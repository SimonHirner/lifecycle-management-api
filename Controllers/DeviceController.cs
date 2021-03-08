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
    /// This endpoint manages all operations for devices.
    /// </summary>
    [Route("api/v1/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private Context context;
        public DeviceController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all devices.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Device[]> GetAllDevices()
        {
            return Ok(context.Devices.ToArray());
        }

        /// <summary>
        /// Returns the device with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Device> GetDevice(int id)
        {
            var device = context.Devices.Where(c => c.DeviceId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Devices WHERE Devices.ID = id TOP 1" -- only first entry or null

            if (device == null) return NotFound();
            return Ok(device);
        }

        /// <summary>
        /// Adds a device.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Device>> AddDevice([FromBody] Device device)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Device) is valid

                //test if device already exists
                if (context.Devices.Where(c => c.DeviceId == device.DeviceId).FirstOrDefault() != null)
                    return Conflict("device with id already exists"); //device with id already exists, we return a conflict

                context.Devices.Add(device);
                //is translated into "INSERT INTO Devices (colum names in Device class) VALUES (values in device object)"

                await context.SaveChangesAsync();

                return Ok(device); //we return the device
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Device
        }

        /// <summary>
        /// Updates a device.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Device>> UpdateDevice([FromBody] Device device)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (device.DeviceId == 0)
                {
                    return BadRequest("When updating a device, the id must be provided.");
                }

                //we try to find the existing device in the database
                var existingDevice = context.Devices.Where(c => c.DeviceId == device.DeviceId).FirstOrDefault();

                if (existingDevice != null)
                { //if the device exists, existingDevice is not null

                    //we delete the existing one
                    context.Devices.Remove(existingDevice);
                    //Translated into "DELETE FROM Devices WHERE Devices.ID = id"

                    //we add the updated one
                    context.Devices.Add(device);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated device
                    return Ok(device);
                }
                else
                {
                    //device doesn't exists
                    return NotFound(); //we couldn't find the device in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a device with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDevice([FromRoute] int id)
        {
            var existingDevice = context.Devices.Where(c => c.DeviceId == id).FirstOrDefault();
            if (existingDevice == null)
            {
                return NotFound();
            }
            else
            {
                //delete device
                context.Remove(existingDevice);
                //Translated into "DELETE FROM Devices WHERE Devices.ID = existingDevice.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}