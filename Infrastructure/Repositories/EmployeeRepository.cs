using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public CanteenEnum GetCanteenById(string employeeId)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);

            if (employee != null)
            {
                return employee.WorkPlace;
            }

            throw new Exception("Employee not found");
        }

        public Employee GetEmployeeById(string Id)
        {
            return _context.Employees.First(p => p.EmployeeID == Id);
        }
    }
}
