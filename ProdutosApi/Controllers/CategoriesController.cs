using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;
using ProdutosApi.Repositories;

namespace ProdutosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            IEnumerable<Category> categories = _repository.GetAll();

            return Ok(categories);
        }


        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
        {
            IEnumerable<Category> categories = _repository.GetCategoriesWithProducts();

            return Ok(categories);
        }


        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            Category category = _repository.Get(id);
            
            if (category is null)
            {
                NotFound("Categoria não encontrada");
            }

            return Ok(category);
        }


        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                return BadRequest("Não é possível cadastrar uma categoria vazia");
            }
            
            //Se o id digitado da categoria já existe no banco de dados, então não deixe cadastrar
            Category existingCategory = _repository.Get(category.Id);

            if (existingCategory != null)
            {
                return BadRequest("Categoria com id já existente");
            }

            _repository.Create(category);

            return new CreatedAtRouteResult("GetCategory", new { id = category.Id }, category);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            var existingCategory = _repository.Get(id);

            if (existingCategory is null)
            {
                return NotFound("Categoria não encontrada");
            }

            if (category.Id != id)
            {
                return BadRequest("Não é possível mudar o valor da chave primária.");
            }

            _repository.Update(category);

            return Ok(category);
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _repository.Get(id);

            if (category is null)
            {
                return NotFound("Categoria não encontrada");
            }

            _repository.Delete(id);

            return Ok();
        }
    }
}
