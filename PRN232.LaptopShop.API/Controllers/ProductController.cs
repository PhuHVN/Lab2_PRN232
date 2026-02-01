using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232.LaptopShop.Repository.Entity;
using PRN232.LaptopShop.Service;
using PRN232.LaptopShop.Service.DTO;

namespace PRN232.LaptopShop.API.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int pageIndex = 1, int pageSize = 10)
        {
            var products = await productService.GetAllProducts(pageIndex, pageSize);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(int categoryId,[FromBody]ProductRequest product)
        {
            var newProduct = await productService.AddProduct(categoryId, product);
            return Ok(newProduct);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest product)
        {
            await productService.UpdateProduct(id,product);   
            return Ok(product);
        }

    }
}
