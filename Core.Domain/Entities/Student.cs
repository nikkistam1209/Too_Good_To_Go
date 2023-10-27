using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Student
    {
        [Key]
        public string? StudentID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DOB { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public CityEnum? City { get; set; }

        public string? Phone { get; set; }

    }
}
