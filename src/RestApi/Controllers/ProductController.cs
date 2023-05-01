using System.Net;
using OneOf.Types;
using RestApi.Models;
using Domain.Entities;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IOutputCacheStore _outputCacheStore;

        public ProductController(IProductService productService, IOutputCacheStore outputCacheStore)
        {
            _productService = productService;
            _outputCacheStore = outputCacheStore;
        }

        /// <summary>
        /// Get list of products
        /// </summary>
        [HttpGet]
        [OutputCache(Duration = 120)]
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
        [OutputCache(Duration = 120)]
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
        [Authorize]
        [OutputCache(Duration = 60)]
        [ProducesResponseType(typeof(Success), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellation)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return await result.Match<Task<IActionResult>>(
                async product =>
                {
                    var result = await _productService.DeleteProductAsync(id);
                    await _outputCacheStore.EvictByTagAsync("tag-all", cancellation);

                    return Ok(result.IsT0);
                },
                noSuchProduct => Task.FromResult<IActionResult>(NotFound(new ErrorResponse(noSuchProduct.Message))));
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="request">Product creation request</param>
        [HttpPost]
        [Authorize]
        [OutputCache(Duration = 60)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreationRequest request, 
            CancellationToken cancellation)
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

            await _outputCacheStore.EvictByTagAsync("tag-all", cancellation);

            return result.Match<IActionResult>(
                product =>
                {
                    return CreatedAtAction(nameof(GetProducts), request);
                },
                alreadyExist => NotFound(new ErrorResponse(alreadyExist.Message)));
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="id">product id</param>
        [HttpPut("productId")]
        [Authorize]
        [OutputCache(Duration = 60)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, 
            UpdateProductRequest request, CancellationToken cancellation)
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

            await _outputCacheStore.EvictByTagAsync("tag-all", cancellation);

            return result.Match<IActionResult>(
                Ok,
                noSuchProduct => NotFound(new ErrorResponse(noSuchProduct.Message)),
                alreadyExist => BadRequest(new ErrorResponse(alreadyExist.Message)));
        }
    }
}
