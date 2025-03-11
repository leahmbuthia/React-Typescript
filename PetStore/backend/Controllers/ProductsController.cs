using backend.Context;
using backend.Dtos;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // CRUD 
        // POST: api/product
        [HttpPost]
        public async Task<ActionResult<ProductEntity>> CreateProduct(CreateProductDto productDto)
        {
            // Convert DTO to Entity
            var product = new ProductEntity
            {
                Brand = productDto.Brand,
                Title = productDto.Title
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            return Ok(product);
        }
        //Get method 
        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductEntity>> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        //Update 
        // PUT: api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute]long id,[FromBody] CreateProductDto dto)
        {

                var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

                if (product == null)
                {
                    return NotFound("Product Not Found");
                }
                product.Title = dto.Title;
                product.Brand = dto.Brand;

                await _context.SaveChangesAsync();
                return Ok("Product Updated successfully");

            
           
        }
        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute]long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

            if (product == null)
            {
                return NotFound("Product Not Found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully");
        }

    }
}
