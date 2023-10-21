using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Core.DomainServices.IServices
{
    public interface ICanteenService
    {
        IEnumerable<Canteen> GetAllCanteens();

        CityEnum? GetCityEnum(CanteenEnum canteenEnum);

        Task<Canteen?> GetCanteenByLocationAsync(CanteenEnum location);
    }
}