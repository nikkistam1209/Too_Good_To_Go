using Core.Domain.Entities;
using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.IRepositories
{
    public interface IEmployeeRepository
    {
        Employee GetEmployeeById(string id);

        CanteenEnum GetCanteenById(string employeeId);

        public Task AddEmployee(Employee employee);

    }
}
