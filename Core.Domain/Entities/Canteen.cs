using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Canteen
    {
        public int Id { get; set; }

        public CityEnum City { get; set; }
        public CanteenEnum Location { get; set; }
        public bool OffersDinners { get; set; }

    }
}
