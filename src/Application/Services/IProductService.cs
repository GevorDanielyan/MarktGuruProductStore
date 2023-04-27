using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using System.Threading.Tasks;


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
        Task<Product> GetProductByIdAsync(Guid id);

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
