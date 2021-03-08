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
    /// This endpoint manages all operations for servers.
    /// </summary>
    [Route("api/v1/models/servers")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private Context context;
        public ServerController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all servers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Server[]> GetAllServers()
        {
            return Ok(context.Servers.Include(s => s.Devices).ToArray());
        }
                
        /// <summary>
        /// Returns the server with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Server> GetServer(int id)
        {
            var server = context.Servers.Where(c => c.ModelId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Servers WHERE Servers.ID = id TOP 1" -- only first entry or null

            if (server == null) return NotFound();
            return Ok(server);
        }

        /// <summary>
        /// Adds a server.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Server>> AddServer([FromBody] Server server)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Server) is valid

                //test if server already exists
                if (context.Servers.Where(c => c.ModelId == server.ModelId).FirstOrDefault() != null)
                    return Conflict("server with id already exists"); //server with id already exists, we return a conflict

                context.Servers.Add(server);
                //is translated into "INSERT INTO Servers (colum names in Server class) VALUES (values in server object)"

                await context.SaveChangesAsync();

                return Ok(server); //we return the server
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Server
        }

        /// <summary>
        /// Updates a server.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Server>> UpdateServer([FromBody] Server server)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (server.ModelId == 0)
                {
                    return BadRequest("When updating a server, the id must be provided.");
                }

                //we try to find the existing server in the database
                var existingServer = context.Servers.Where(c => c.ModelId == server.ModelId).FirstOrDefault();

                if (existingServer != null)
                { //if the server exists, existingServer is not null

                    //we delete the existing one
                    context.Servers.Remove(existingServer);
                    //Translated into "DELETE FROM Servers WHERE Servers.ID = id"

                    //we add the updated one
                    context.Servers.Add(server);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated server
                    return Ok(server);
                }
                else
                {
                    //server doesn't exists
                    return NotFound(); //we couldn't find the server in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a server with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteServer([FromRoute] int id)
        {
            var existingServer = context.Servers.Where(c => c.ModelId == id).FirstOrDefault();
            if (existingServer == null)
            {
                return NotFound();
            }
            else
            {
                //delete server
                context.Remove(existingServer);
                //Translated into "DELETE FROM Servers WHERE Servers.ID = existingServer.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}