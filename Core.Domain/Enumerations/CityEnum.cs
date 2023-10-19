using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Enumerations
{
    public enum CityEnum
    {
        Breda = 1,

        [Display(Name = "Den Bosch")]
        DenBosch = 2,

        Tilburg = 3
    }
}
