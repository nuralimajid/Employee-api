using cifor.Data;
using cifor.DTOs;
using cifor.Models;
using Microsoft.EntityFrameworkCore;

namespace cifor.Service;

public class EmployeeServices: IEmployeeService
{
        
        private readonly AppDbContext _context;
        public EmployeeServices(AppDbContext context) => _context = context;
        //get All Data
        public async Task<List<Employee>> GetAllAsync() => await _context.Employees.ToListAsync();
        
        //get Data By Id
        public async Task<Employee?> GetByIdAsync(string id) => await _context.Employees.FindAsync(id);

        public async Task<List<Employee>> SearchAsync(string? name, string? department)
        {
                var query = _context.Employees.AsQueryable();
                
                if(!string.IsNullOrWhiteSpace(name)) 
                        query = query.Where(n=>EF.Functions.ILike(n.Name, $"%{name}%"));
                
                if(!string.IsNullOrWhiteSpace(department))
                        query = query.Where(d => EF.Functions.ILike(d.Department, department));
                
                return await query.ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
                var employee = await _context.Employees.FindAsync(id);
                if(employee == null ) return false;
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return true;
        }
        
        
}