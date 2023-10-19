using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Give the product a name")]
        public string? Name { get; set; }

        public bool ContainsAlcohol { get; set; }

        public string? Picture { get; set; }

        public ICollection<Package>? Packages { get; set; } = new List<Package>();

    }
}
