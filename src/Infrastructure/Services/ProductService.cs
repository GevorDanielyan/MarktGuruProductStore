using OneOf;
using OneOf.Types;
using Domain.Entities;
using Application.Services;
using Domain.Exceptions.Structs;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IDbService _dbService;
        private readonly ILogger _logger;

        public ProductService(IDbService dbService, ILogger<ProductService> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        public async Task<OneOf<Success, AlreadyExistedProduct>> AddProductAsync(string name, double price, bool available, string? description)
        {
            try
            {
                string command = "SELECT * FROM public.product WHERE name = @Name";

                // Checking if a product with the same name already exists
                var existingProduct = await _dbService.GetAsync<Product>(command, new { name });
                if (existingProduct is not null)
                {
                    return new AlreadyExistedProduct(name);
                }
                command = "INSERT INTO public.product (name, available, price, description) VALUES (@Name, @Available, @Price, @Description)";
                var result = await _dbService.EditData(command, new { Name = name, Available = available, Price = price, Description = description });

                return new Success();
            }
            catch (Exception error)
            {
                _logger.LogError("Error occured when trying to insert new product", error);
                throw;
            }
        }

        public async Task<OneOf<Success, NoSuchProduct>> DeleteProductAsync(Guid id)
        {
            try
            {
                string command = "DELETE FROM public.product WHERE id=@Id";
                var deleteProduct = await _dbService.EditData(command, new { id });
                return new Success();
            }
            catch (Exception error)
            {
                _logger.LogError("Error occured when trying to delete product", error);
                throw;
            }
        }

        public async Task<OneOf<Product, NoSuchProduct>> GetProductByIdAsync(Guid id)
        {
            try
            {
                string command = "SELECT * FROM public.product where id=@id";
                var product = await _dbService.GetAsync<Product>(command, new { id });
                if (product is null)
                {
                    return new NoSuchProduct(id);
                }
                return product;
            }
            catch (Exception error)
            {
                _logger.LogError("Error occured while trying to get product by id", error);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductListAsync()
        {
            try
            {
                string command = "SELECT * FROM public.product";
                var products = await _dbService.GetAll<Product>(command, new { });

                return products;
            }
            catch (Exception error)
            {
                _logger.LogError("Error occured while trying to get product list", error);
                throw;
            }

        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            try
            {
                var result = await _dbService.EditData("Update public.product SET name=@Name, price=@Price, available=@Available, description=@Description WHERE id=@Id", product);
                return product;
            }
            catch (Exception error)
            {
                _logger.LogError("Error occured while trying to update product", error);
                throw;
            }

        }
    }
}