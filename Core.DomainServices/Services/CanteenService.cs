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
    public class CanteenService : ICanteenService
    {
        private readonly ICanteenRepository _canteenRepository;

        public CanteenService(ICanteenRepository canteenRepository)
        {
            _canteenRepository = canteenRepository;
        }

        public IEnumerable<Canteen> GetAllCanteens()
        {
            return _canteenRepository.GetAllCanteens();
        }

        public CityEnum? GetCityEnum(CanteenEnum canteenEnum)
        {
            return _canteenRepository.GetCityEnum(canteenEnum);
        }

        public async Task<Canteen?> GetCanteenByLocationAsync(CanteenEnum location)
        {
            var canteen = await _canteenRepository.GetCanteenByLocation(location);

            if (canteen == null)
                throw new Exception("Canteen not found");

            return canteen;
        }
    }
}
