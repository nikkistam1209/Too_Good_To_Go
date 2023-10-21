using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CanteenRepository : ICanteenRepository
    {
        private readonly ApplicationDbContext _context;

        public CanteenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Canteen> GetAllCanteens()
        {
            return _context.Canteens;
        }

        public CityEnum? GetCityEnum(CanteenEnum canteenEnum)
        {
            var canteen = _context.Canteens.FirstOrDefault(c => c.Location == canteenEnum);

            if (canteen != null)
            {
                return canteen.City;
            }
            return null;
        }

        public Canteen GetCanteenById(int id)
        {
            return _context.Canteens.First(p => p.Id == id);
        }

        public async Task<Canteen?> GetCanteenByLocation(CanteenEnum location)
        {
            return await _context.Canteens.Where(c => c.Location == location).FirstOrDefaultAsync();
        }

    }
}
