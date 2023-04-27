using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using System.Net;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProductListAsync();

            return Ok(result);
        }

        
        /// <summary>
        /// Get product by given product id
        /// </summary>
        /// <param name="id">Product id</param>
        [HttpGet("productId")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductByProductId(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return result.Match<IActionResult>(
                Ok,
                noSuchProduct => NotFound(new ErrorResponse(noSuchProduct.Message)));

        }
    }
}
