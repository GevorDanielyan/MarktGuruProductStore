using Domain.Entities;
using Domain.Exceptions.Structs;
using OneOf;

namespace Application.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Add new product
        /// </summary>
        Task<bool> AddProductAsync(Product product);
        
        /// <summary>
        /// Get all products
        /// </summary>
        Task<IEnumerable<Product>> GetProductListAsync();

        /// <summary>
        /// Get product by Id
        /// </summary>
        Task<OneOf<Product, NoSuchProduct>> GetProductByIdAsync(Guid id);

        /// <summary>
        /// Update product
        /// </summary>
        Task<Product> UpdateProductAsync(Product product);

        /// <summary>
        /// Delete product by product Id
        /// </summary>
        Task<bool> DeleteProductAsync(Guid id);
    }
}
