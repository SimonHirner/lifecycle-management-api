using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifecycleManagementAPI.Controllers
{
    /// <summary>
    /// This endpoint manages all operations for categories.
    /// </summary>
    [Route("api/v1/categories")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private Context context;
        public CategorieController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category[]> GetAllCategories()
        {
            return Ok(context.Categories.ToArray());
        }
    }
}