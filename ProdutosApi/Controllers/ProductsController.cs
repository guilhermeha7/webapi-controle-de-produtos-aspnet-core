using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;
using System;

namespace ProdutosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get() //ActionResult retorna um status code, além do tipo especificado
        {
            var products = _context.Produtos.ToList();

            if (products is null)
            {
                return NotFound("Nenhum produto foi encontrado");
            }

            return products;
        }

        [HttpGet("{id:int}", Name = "GetProduct")] //O valor digitado pelo usuário é capturado na variável id e injetado automaticamente no parâmetro id do método
        public ActionResult<Product> Get(int id)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Produto não encontrado");
            }

            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product) //No parâmetro do método Post ou Put se coloca o body
        {
            if (product is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(product);
            _context.SaveChanges();

            //Retorna 201 Created, inclui no cabeçalho da resposta a URL do recurso e envia o produto no body da response
            return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)  
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified; //Marca esta entidade como modificada, para que o EF gere um UPDATE no banco usando o Id dela como chave
            _context.SaveChanges();

            return Ok(product); //No parâmetro do método Ok se coloca o vai ser mostrado na response
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Produto não localizado");
            }

            _context.Produtos.Remove(product);
            _context.SaveChanges();

            return Ok();
        }
    }
}
