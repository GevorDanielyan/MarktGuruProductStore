using Domain.Entities;
using Domain.Exceptions.Structs;
using OneOf;
using OneOf.Types;

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
        Task<OneOf<Success, NoSuchProduct>> DeleteProductAsync(Guid id);
    }
}
