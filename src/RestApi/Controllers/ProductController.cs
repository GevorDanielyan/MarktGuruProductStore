using System.Net;
using OneOf.Types;
using RestApi.Models;
using Domain.Entities;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Get list of products
        /// </summary>
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

        /// <summary>
        /// Delete product by given product id
        /// </summary>
        /// <param name="id">Product id</param>
        [HttpDelete("productId")]
        [ProducesResponseType(typeof(Success), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return await result.Match<Task<IActionResult>>(
                async product =>
                {
                    var result = await _productService.DeleteProductAsync(id);
                    
                    return Ok(result.IsT0);
                },
                noSuchProduct => Task.FromResult<IActionResult>(NotFound(new ErrorResponse(noSuchProduct.Message))));
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="request">Product creation request</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreationRequest request)
        {
            if (request is null 
                || string.IsNullOrWhiteSpace(request.Name) 
                || request.Price <= 0)
            {
                return BadRequest();
            }

            var result = await _productService.AddProductAsync(request.Name,
                request.Price,
                request.Available,
                request.Description ?? string.Empty);

            return result.Match<IActionResult>(
                product =>
                {
                    return CreatedAtAction(nameof(GetProducts), request);
                },
                alreadyExist => NotFound(new ErrorResponse(alreadyExist.Message)));
        }

        [HttpPut("productId")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, 
            UpdateProductRequest request)
        {
            if (request is null
                || string.IsNullOrWhiteSpace(request.Name)
                || request.Price <= 0)
            {
                return BadRequest();
            }
            var result = await _productService.UpdateProductAsync(id,
                request.Name, 
                request.Price, 
                request.Available, 
                request.Description);

            return result.Match<IActionResult>(
                Ok,
                noSuchProduct => NotFound(new ErrorResponse(noSuchProduct.Message)),
                alreadyExist => BadRequest(new ErrorResponse(alreadyExist.Message)));
        }
    }
}
