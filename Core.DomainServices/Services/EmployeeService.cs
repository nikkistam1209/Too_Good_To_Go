using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task AddEmployee(Employee employee)
        {
            try
            {
                await _employeeRepository.AddEmployee(employee);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding employee: " + ex.Message, ex);
            }
        }

        public CanteenEnum GetCanteenById(string id)
        {
            try
            {
                return _employeeRepository.GetCanteenById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error finding canteen: " + ex.Message, ex);
            }
        }


        public Employee GetEmployeeById(string id)
        {
            try
            {
                return _employeeRepository.GetEmployeeById(id);
            }
            catch
            {
                throw new Exception("Employee could not be found");
            }
        }
    }
}
