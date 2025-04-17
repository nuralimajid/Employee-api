using cifor.DTOs;
using cifor.Models;

namespace cifor.Service;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(string id);
    Task<List<Employee>> SearchAsync(string? name, string? department);
    Task AddAsync(Employee employee);
    Task<bool> DeleteAsync(string id);
}