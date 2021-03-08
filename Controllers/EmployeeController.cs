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
    /// This endpoint manages all operations for employees.
    /// </summary>
    [Route("api/v1/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private Context context;
        public EmployeeController(Context context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns all employees.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Employee[]> GetAllEmployees()
        {
            return Ok(context.Employees.Include(e => e.Activities).ToArray());
        }

        /// <summary>
        /// Returns the employee with a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = context.Employees.Where(c => c.EmployeeId == id).FirstOrDefault();
            //statement is translated into "SELECT * FROM Employees WHERE Employees.ID = id TOP 1" -- only first entry or null

            if (employee == null) return NotFound();
            return Ok(employee);
        }

        /// <summary>
        /// Adds a employee.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Employee>> AddEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            { //Tests, if the data model (in our case Employee) is valid

                //test if employee already exists
                if (context.Employees.Where(c => c.EmployeeId == employee.EmployeeId).FirstOrDefault() != null)
                    return Conflict("employee with id already exists"); //employee with id already exists, we return a conflict

                context.Employees.Add(employee);
                //is translated into "INSERT INTO Employees (colum names in Employee class) VALUES (values in employee object)"

                await context.SaveChangesAsync();

                return Ok(employee); //we return the employee
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Employee
        }

        /// <summary>
        /// Updates a employee.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> UpdateEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {

                //test if id is provided
                if (employee.EmployeeId == 0)
                {
                    return BadRequest("When updating a employee, the id must be provided.");
                }

                //we try to find the existing employee in the database
                var existingEmployee = context.Employees.Where(c => c.EmployeeId == employee.EmployeeId).FirstOrDefault();

                if (existingEmployee != null)
                { //if the employee exists, existingEmployee is not null

                    //we delete the existing one
                    context.Employees.Remove(existingEmployee);
                    //Translated into "DELETE FROM Employees WHERE Employees.ID = id"

                    //we add the updated one
                    context.Employees.Add(employee);

                    //save changes in the database
                    await context.SaveChangesAsync();

                    //we return the updated employee
                    return Ok(employee);
                }
                else
                {
                    //employee doesn't exists
                    return NotFound(); //we couldn't find the employee in our database
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a employee with the given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteEmployee([FromRoute] int id)
        {
            var existingEmployee = context.Employees.Where(c => c.EmployeeId == id).FirstOrDefault();
            if (existingEmployee == null)
            {
                return NotFound();
            }
            else
            {
                //delete employee
                context.Remove(existingEmployee);
                //Translated into "DELETE FROM Employees WHERE Employees.ID = existingEmployee.Id"

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}