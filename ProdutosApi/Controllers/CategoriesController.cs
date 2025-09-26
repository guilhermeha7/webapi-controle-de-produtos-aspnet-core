using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;

namespace ProdutosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAsync()
        {
            var categories = await _context.Categorias.ToListAsync();

            return categories;
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesWithProductsAsync()
        {
            return await _context.Categorias.AsNoTracking().Include(c => c.Products).ToListAsync();
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetAsync(int id)
        {
            var category = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(p =>  p.Id == id);
            
            if (category is null)
            {
                NotFound("Categoria não encontrada");
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Category category)
        {
            if (category is null)
            {
                return BadRequest("Não é possível cadastrar uma categoria vazia");
            }

            _context.Categorias.Add(category);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetCategory", new { id = category.Id }, category);
            //return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Category category)
        {
            var existingCategory = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory is null)
            {
                return NotFound("Categoria não encontrada");
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            existingCategory.Name = category.Name;
            existingCategory.ImageUrl = category.ImageUrl;

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var category = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound("Categoria não encontrada");
            }

            _context.Categorias.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
