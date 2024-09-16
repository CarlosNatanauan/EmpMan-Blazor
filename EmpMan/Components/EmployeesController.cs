using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmpMan.Data;
using EmpMan.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpMan.Components
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmpManDBContext _context;

        public EmployeesController(EmpManDBContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var employees = await _context.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/Employees/
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // PUT: api/Employees/
        [HttpPut("{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("Employee ID mismatch.");
            }

            try
            {
                await _context.UpdateEmployeeAsync(id, employee.FName, employee.LName, employee.Email);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }




        // POST: api/Employees
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.CreateEmployeeAsync(employee.FName, employee.LName, employee.Email);

            var newEmployee = await _context.GetEmployeeByIdAsync(employee.Id);

            return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee?.Id }, newEmployee);
        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _context.DeleteEmployeeAsync(id);

            return NoContent();  
        }

        private async Task<bool> EmployeeExists(int id)
        {
            var employee = await _context.GetEmployeeByIdAsync(id);
            return employee != null;
        }
    }
}
