using cifor.Models;
using cifor.Service;
using Microsoft.AspNetCore.Mvc;

namespace cifor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService services;

    public EmployeeController(IEmployeeService service) => services = service;

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetAllEmployee()
    {
        var Employe = await services.GetAllAsync();
        if (Employe == null) return NotFound($"Employee not found");
        return Ok(Employe);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return BadRequest("Employee id cannot be empty");

        var employee = await services.GetByIdAsync(id);
        if (employee == null) return NotFound($"Employee with id {id} not found");
        return Ok(employee);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Employee>>> SearchEmployee(string? name, string? department)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(department))
            return BadRequest("At least one search parameter (name or departement) is required.");

        var result = await services.SearchAsync(name, department);

        if (result.Count == 0)
        {
            return NotFound($"Employee with id {name} not found");
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.EmployeeId) && string.IsNullOrWhiteSpace(employee.Name))
            return BadRequest("Employee id and name cannot be empty");

        var exits = await services.GetByIdAsync(employee.EmployeeId);
        if (exits != null) return BadRequest("Employee already exists");

        await services.AddAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) return BadRequest("EmployeeId is required");

        var result = await services.DeleteAsync(id);
        return result ? NoContent() : NotFound($"Employee with EmployeeId {id} not found");
    }
}