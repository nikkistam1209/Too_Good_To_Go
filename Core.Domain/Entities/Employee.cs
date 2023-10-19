using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Employee
    {
        [Key]
        public string? EmployeeID { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public CanteenEnum WorkPlace { get; set; }

    }
}
