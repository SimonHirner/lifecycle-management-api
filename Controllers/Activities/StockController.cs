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
    /// This endpoint manages all operations for stocks.
    /// </summary>
    [Route("api/v1/activities/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private Context context;
        public StockController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all stocks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Stock[]> GetAllStocks()
        {
            return Ok(context.Stocks.Include(s => s.Devices).ToArray());
        }
        
        /// <summary>
        /// Returns the stock with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Stock> GetStock(int id)
        {
            var stock = context.Stocks.Where(c => c.ActivityId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Stocks WHERE Stocks.ID = id TOP 1" -- only first entry or null

            if (stock == null) return NotFound();
            return Ok(stock);
        }

        /// <summary>
        /// Adds a stock.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Stock>> AddStock([FromBody] Stock stock)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Stock) is valid

                //test if stock already exists
                if (context.Stocks.Where(c => c.ActivityId == stock.ActivityId).FirstOrDefault() != null)
                    return Conflict("stock with id already exists"); //stock with id already exists, we return a conflict

                context.Stocks.Add(stock);
                //is translated into "INSERT INTO Stocks (colum names in Stock class) VALUES (values in stock object)"

                await context.SaveChangesAsync();

                return Ok(stock); //we return the stock
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Stock
        }

        /// <summary>
        /// Updates a stock.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Stock>> UpdateStock([FromBody] Stock stock)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (stock.ActivityId == 0)
                {
                    return BadRequest("When updating a stock, the id must be provided.");
                }

                //we try to find the existing stock in the database
                var existingStock = context.Stocks.Where(c => c.ActivityId == stock.ActivityId).FirstOrDefault();

                if (existingStock != null)
                { //if the stock exists, existingStock is not null

                    //we delete the existing one
                    context.Stocks.Remove(existingStock);
                    //Translated into "DELETE FROM Stocks WHERE Stocks.ID = id"

                    //we add the updated one
                    context.Stocks.Add(stock);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated stock
                    return Ok(stock);
                }
                else
                {
                    //stock doesn't exists
                    return NotFound(); //we couldn't find the stock in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a stock with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStock([FromRoute] int id)
        {
            var existingStock = context.Stocks.Where(c => c.ActivityId == id).FirstOrDefault();
            if (existingStock == null)
            {
                return NotFound();
            }
            else
            {
                //delete stock
                context.Remove(existingStock);
                //Translated into "DELETE FROM Stocks WHERE Stocks.ID = existingStock.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}