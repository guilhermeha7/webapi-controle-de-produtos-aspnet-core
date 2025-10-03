using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;
using ProdutosApi.Repositories;
using System;

namespace ProdutosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        //É necessário definir IConfiguration como dependência para conseguir pegar valores de chaves do arquivo appsettings.json 
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;


        public ProductsController(IConfiguration configuration, ILogger<ProductsController> logger, IProductRepository repository) //O <T> de ILogger<ProductsController> serve só para dizer de qual classe está vindo o log 
        {
            _configuration = configuration;
            _logger = logger;
            _repository = repository;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get() //ActionResult<T> retorna status codes, além do tipo especificado
        {
            _logger.LogInformation("::::::::::::::: GET /products :::::::::::::::");

            var products = _repository.GetAll();
            
            return Ok(products);
        }


        [HttpGet("{id:int}", Name = "GetProduct")] //O valor digitado pelo usuário é capturado na variável id e injetado automaticamente no parâmetro id do método
        public ActionResult<Product> Get(int id)
        {
            var product = _repository.GetByIdAsNoTracking(p => p.Id == id);

            return Ok(product);
        }

        [HttpGet("category/{categoryId:int}")]
        public ActionResult<IEnumerable<Product>> GetProductsFromCategoryId(int categoryId)
        {
            IEnumerable<Product> products = _repository.GetProductsFromCategoryId(categoryId);
            
            return Ok(products);
        }


        [HttpPost]
        public ActionResult Post([FromBody] Product product) //No parâmetro do método Post ou Put se coloca o body
        {
            if (product is null)
            {
                BadRequest("Não é possível cadastrar uma categoria vazia");
            }

            Product existingProduct = _repository.GetByIdAsNoTracking(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                BadRequest("Um produto com esse id já existe no banco de dados");
            }

            _repository.Create(product);

            //Retorna 201 Created, inclui no cabeçalho da resposta a URL do recurso e envia o produto no body da response
            return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Não é possível mudar o valor da chave primária.");
            }

            var existingProduct = _repository.GetByIdAsNoTracking(p => p.Id == id);

            if (existingProduct is null)
            {
                return NotFound("Produto não encontrado");
            }

            _repository.Update(product);

            return Ok(product); //No parâmetro do método Ok se coloca o vai ser mostrado na response
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var existingProduct = _repository.GetById(p => p.Id == id);

            if (existingProduct is null)
            {
                return NotFound("Produto não encontrado");
            }

            _repository.Delete(existingProduct);

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
