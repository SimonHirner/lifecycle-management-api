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

        /*
        /// <summary>
        /// Adds a model.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Model>> AddModel([FromBody] Model model)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Model) is valid

                //test if model already exists
                if (context.Models.Where(c => c.ModelId == model.ModelId).FirstOrDefault() != null)
                    return Conflict("model with id already exists"); //model with id already exists, we return a conflict

                context.Models.Add(model);
                //is translated into "INSERT INTO Models (colum names in Model class) VALUES (values in model object)"

                await context.SaveChangesAsync();

                return Ok(model); //we return the model
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Model
        }

        /// <summary>
        /// Updates a model.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Model>> UpdateModel([FromBody] Model model)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (model.ModelId == 0)
                {
                    return BadRequest("When updating a model, the id must be provided.");
                }

                //we try to find the existing model in the database
                var existingModel = context.Models.Where(c => c.ModelId == model.ModelId).FirstOrDefault();

                if (existingModel != null)
                { //if the model exists, existingModel is not null

                    //we delete the existing one
                    context.Models.Remove(existingModel);
                    //Translated into "DELETE FROM Models WHERE Models.ID = id"

                    //we add the updated one
                    context.Models.Add(model);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated model
                    return Ok(model);
                }
                else
                {
                    //model doesn't exists
                    return NotFound(); //we couldn't find the model in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a model with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteModel([FromRoute] int id)
        {
            var existingModel = context.Models.Where(c => c.ModelId == id).FirstOrDefault();
            if (existingModel == null)
            {
                return NotFound();
            }
            else
            {
                //delete model
                context.Remove(existingModel);
                //Translated into "DELETE FROM Models WHERE Models.ID = existingModel.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
        */
    }
}