using Core.Domain.Entities;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public IEnumerable<Product> GetProductsByIds(List<int> productIds)
        {
            return _productRepository.GetProductsByIds(productIds);
        }


        public Product GetProductById(int id)
        {
            try
            {
                return _productRepository.GetProductById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error finding product: " + ex.Message, ex);
            }
        }
    }
}
