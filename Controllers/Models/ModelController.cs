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
    /// This endpoint manages all operations for models.
    /// </summary>
    [Route("api/v1/models")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private Context context;
        public ModelController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all models.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Model[]> GetAllModels()
        {
            return Ok(context.Models.Include(m => m.Devices).ToArray());
        }

        /// <summary>
        /// Returns the model with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Model> GetModel(int id)
        {
            var model = context.Models.Where(c => c.ModelId == id).Include(a => a.Devices).FirstOrDefault();
            //statement is translated into "SELECT * FROM Models WHERE Models.ID = id TOP 1" -- only first entry or null

            if (model == null) return NotFound();
            return Ok(model);
        }
    }
}