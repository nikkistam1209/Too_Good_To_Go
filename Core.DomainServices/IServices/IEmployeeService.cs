using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Core.DomainServices.IServices
{
    public interface IEmployeeService
    {
        Employee GetEmployeeById(string id);

        CanteenEnum GetCanteenById(string id);
        Task AddEmployee(Employee employee);
    }
}