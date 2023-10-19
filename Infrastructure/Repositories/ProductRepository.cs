using Core.Domain.Entities;
using Core.DomainServices.IRepositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products;
        }

        public Product GetProductById(int id)
        {
            return _context.Products.First(p => p.Id == id);
        }

        public IEnumerable<Product> GetProductsByIds(IEnumerable<int> productIds)
        {
            return _context.Products.Where(p => productIds.Contains(p.Id)).ToList();
        }



    }
}
