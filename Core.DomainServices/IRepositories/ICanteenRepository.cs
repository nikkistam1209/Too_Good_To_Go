using Core.Domain.Entities;
using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.IRepositories
{
    public interface ICanteenRepository
    {
        IEnumerable<Canteen> GetAllCanteens();

        Canteen GetCanteenById(int id);
        CityEnum? GetCityEnum(CanteenEnum canteenEnum);

        Task<Canteen?> GetCanteenByLocation(CanteenEnum location);
    }
}
