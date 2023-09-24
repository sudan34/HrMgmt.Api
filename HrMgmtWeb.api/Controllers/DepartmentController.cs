using HrMgmtWeb.api.Data;
using HrMgmtWeb.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrMgmtWeb.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var Department = _context.Department.ToList();
            return Ok(Department);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            _context.Department.Add(department);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = department.DeptId }, department);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Department department)
        {
            if (department == null || id != department.DeptId)
            {
                return BadRequest();
            }

            var existingDepartment = _context.Department.Find(id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            existingDepartment.DeptName = department.DeptName;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var department = _context.Department.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Department.Remove(department);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
