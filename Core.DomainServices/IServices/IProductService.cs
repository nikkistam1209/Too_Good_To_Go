using Core.Domain.Entities;

namespace Core.DomainServices.IServices
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByIds(List<int> productIds);
        Product GetProductById(int id);
    }
}