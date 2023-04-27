using Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IDbService _dbService;
        public ProductService(IDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            var result = await _dbService.EditData(
                "INSERT INTO public.employee (id,name, age, address, mobile_number) VALUES (@Id, @Name, @Age, @Address, @MobileNumber)",
                product);
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var deleteProduct = await _dbService.EditData("DELETE FROM public.employee WHERE id=@Id", new { id });
            return true;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var productList = await _dbService.GetAsync<Product>("SELECT * FROM public.employee where id=@id", new { id });
            return productList;
        }

        public async Task<IEnumerable<Product>> GetProductListAsync()
        {
            var productList = await _dbService.GetAll<Product>("SELECT * FROM public.product", new { });
            return productList;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var updateProdut =
            await _dbService.EditData(
                "Update public.employee SET name=@Name, age=@Age, address=@Address, mobile_number=@MobileNumber WHERE id=@Id",
                product);
            return product;
        }
    }
}
