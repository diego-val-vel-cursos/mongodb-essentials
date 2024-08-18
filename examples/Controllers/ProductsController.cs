using examples.Models;
using examples.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace examples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get() =>
            await _productService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _productService.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _productService.CreateAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id.ToString() }, product);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Product productIn)
        {
            var product = await _productService.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.UpdateAsync(id, productIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productService.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet("indexes")]
        public async Task<ActionResult<List<string>>> GetIndexes()
        {
            // Obtener la lista de índices de la colección
            var indexes = await _productService.GetIndexesAsync();
            var indexList = new List<string>();

            foreach (var index in indexes)
            {
                // Convertir cada índice a formato JSON y agregarlo a la lista
                indexList.Add(index.ToJson());
            }

            return Ok(indexList);
        }
    }
}
