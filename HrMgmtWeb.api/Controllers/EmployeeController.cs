using HrMgmtWeb.api.Data;
using HrMgmtWeb.api.Models;
using HrMgmtWeb.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrMgmtWeb.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeWithDepartmentDTO>>> GetEmployee()
        {
            var employees = await _context.Employee
                .Include(e => e.Department) // Include the Department navigation property
                .ToListAsync();

            // Create a list of DTOs with employee and department details
            var employeeDTOs = employees.Select(employee => new EmployeeWithDepartmentDTO
            {
                EmpId = employee.EmpId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                Email = employee.Email,
                Phone = employee.Phone,
                DeptId = employee.DeptId,
                DeptName = employee.Department?.DeptName // Access the department name
            }).ToList();

            return employeeDTOs;
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeWithDepartmentDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employee.Include(e => e.Department) // Include the Department navigation property
        .FirstOrDefaultAsync(e => e.EmpId == id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDTO = new EmployeeWithDepartmentDTO
            {
                EmpId = employee.EmpId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                Email = employee.Email,
                Phone = employee.Phone,
                DeptId = employee.DeptId,
                DeptName = employee.Department?.DeptName // Access the department name
            };
            return employeeDTO;
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            var department = await _context.Department.FindAsync(employee.DeptId);

            if (department == null)
            {
                return BadRequest("Invalid department ID.");
            }
            employee.Department = department;

            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmpId }, employee);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmpId)
            {
                return BadRequest();
            }

            var existingEmployee = await _context.Employee.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Update the employee's properties with values from the request
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Address = employee.Address;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;

            // Check if the DeptId property has changed
            if (existingEmployee.DeptId != employee.DeptId)
            {
                var newDepartment = await _context.Department.FindAsync(employee.DeptId);

                if (newDepartment == null)
                {
                    // Handle the case where the new department with the provided ID doesn't exist
                    return BadRequest("Invalid department ID.");
                }

                // Update the department association
                existingEmployee.DeptId = employee.DeptId;
                existingEmployee.Department = newDepartment;
            }

            _context.Entry(existingEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(existingEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmpId == id);
        }
    }
}
