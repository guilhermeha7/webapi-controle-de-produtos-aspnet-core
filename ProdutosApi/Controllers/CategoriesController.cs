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
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _context.Categorias.ToList();

            return categories;
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
        {
            return _context.Categorias.Include(c => c.Products).ToList();
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            var category = _context.Categorias.FirstOrDefault(p =>  p.Id == id);
            
            if (category is null)
            {
                NotFound("Categoria não encontrada");
            }

            return category;
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                return BadRequest("Não é possível cadastrar uma categoria vazia");
            }

            _context.Categorias.Add(category);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCategory", new { id = category.Id }, category);
            //return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            var existingCategory = _context.Categorias.FirstOrDefault(c => c.Id == id);

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

            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categorias.FirstOrDefault(c => c.Id == id);

            if (category is null)
            {
                return NotFound("Categoria não encontrada");
            }

            _context.Categorias.Remove(category);
            _context.SaveChanges();

            return Ok();
        }
    }
}
