using OneOf;
using OneOf.Types;
using Domain.Entities;
using Domain.Exceptions.Structs;

namespace Application.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Add new product
        /// </summary>
        Task<OneOf<Success, AlreadyExistedProduct>> AddProductAsync(string name, double price, bool available, string description);
        
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
        Task<OneOf<Product, NoSuchProduct, AlreadyExistedProduct>> UpdateProductAsync(Guid id, string name, double price, bool available, string? description);

        /// <summary>
        /// Delete product by product Id
        /// </summary>
        Task<OneOf<Success, NoSuchProduct>> DeleteProductAsync(Guid id);
    }
}
