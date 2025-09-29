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
        //É necessário definir IConfiguration como dependência para conseguir pegar valores de chaves do arquivo appsettings.json 
        private readonly IConfiguration _configuration; 


        public ProductsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAsync() //ActionResult<T> retorna status codes, além do tipo especificado
        {
            var products = await _context.Produtos.AsNoTracking().ToListAsync();

            if (products is null)
            {
                return NotFound("Nenhum produto foi encontrado");
            }

            return products;
        }


        [HttpGet("{id:int}", Name = "GetProduct")] //O valor digitado pelo usuário é capturado na variável id e injetado automaticamente no parâmetro id do método
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            var product = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Produto não encontrado");
            }

            return product;
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Product product) //No parâmetro do método Post ou Put se coloca o body
        {
            if (product is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(product);
            await _context.SaveChangesAsync();

            //Retorna 201 Created, inclui no cabeçalho da resposta a URL do recurso e envia o produto no body da response
            return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified; //Marca esta entidade como modificada, para que o EF gere um UPDATE no banco usando o Id dela como chave
            await _context.SaveChangesAsync();

            return Ok(product); //No parâmetro do método Ok se coloca o vai ser mostrado na response
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var product = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound("Produto não localizado");
            }

            _context.Produtos.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("ExibindoValoresDeAppSettings")]
        public string GetAppSettingsValues()
        {
            string chave1 = _configuration["chave1"];
            string chave1Secao = _configuration["secao1:chave1"];
            return $"Chave 1: {chave1}\n"+
                   $"Chave1Secao: {chave1Secao}";
        }


        [HttpGet("TratamentoDeExcecaoInesperada")]
        public ActionResult GetUnexpectedException()
        {
            throw new Exception("Erro inesperado");
        }
    }
}
