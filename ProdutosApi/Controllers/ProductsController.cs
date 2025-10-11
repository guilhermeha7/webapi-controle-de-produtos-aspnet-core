using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProdutosApi.Context;
using ProdutosApi.DTOs;
using ProdutosApi.Models;
using ProdutosApi.Pagination;
using ProdutosApi.Repositories;
using System;
using System.Linq;

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
        private readonly IMapper _mapper;

        public ProductsController(IConfiguration configuration, ILogger<ProductsController> logger, IProductRepository repository, IMapper mapper) //O <T> de ILogger<ProductsController> serve só para dizer de qual classe está vindo o log 
        {
            _configuration = configuration;
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<ProductViewDto>> Get() //ActionResult<T> retorna status codes, além do tipo especificado
        {
            _logger.LogInformation("::::::::::::::: GET /products :::::::::::::::");

            var products = _repository.GetAll();

            //var destino = _mapper.Map<Destino>(origem);
            var productsViewDto = _mapper.Map<IEnumerable<ProductViewDto>>(products);

            return Ok(productsViewDto);
        }


        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProductViewDto>> Get([FromQuery] PaginationParameters paginationParams)
        {
            var products = _repository.GetWithPagination(paginationParams, p => p.Id);

            int currentPage = paginationParams.PageNumber;
            int pageSize = paginationParams.PageSize;
            int totalItems = _repository.GetTotalItems();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); //(double) força o resultado a vir com casas decimais, tipo 23 / 10 = 2.3. e o Math.Ceiling() arredonda pra cima — ou seja, 2.3 vira 3. Em português claro: “se sobrar algum item, cria mais uma página pra ele”.

            PaginationHeaders paginationHeaders = new PaginationHeaders(currentPage, totalPages, pageSize, totalItems);

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationHeaders)); //Converte um objeto C# em JSON

            var productsViewDto = _mapper.Map<IEnumerable<ProductViewDto>>(products);
            return Ok(productsViewDto);
        }   


        [HttpGet("{id:int}", Name = "GetProduct")] //O valor digitado pelo usuário é capturado na variável id e injetado automaticamente no parâmetro id do método
        public ActionResult<ProductViewDto> Get(int id)
        {
            var product = _repository.GetByIdAsNoTracking(p => p.Id == id);

            var productViewDto = _mapper.Map<ProductViewDto>(product);

            return Ok(productViewDto);
        }


        [HttpGet("category/{categoryId:int}")]
        public ActionResult<IEnumerable<ProductViewDto>> GetProductsFromCategoryId(int categoryId)
        {
            IEnumerable<Product> products = _repository.GetProductsFromCategoryId(categoryId);

            var productsViewDto = _mapper.Map<IEnumerable<ProductViewDto>>(products);

            return Ok(productsViewDto);
        }


        [HttpPost]
        public ActionResult<ProductViewDto> Post([FromBody] ProductInputDto productInputDto) //No parâmetro do método Post ou Put se coloca o body
        {
            if (productInputDto is null)
            {
                return BadRequest("Não é possível cadastrar uma categoria vazia");
            }

            var product = _mapper.Map<Product>(productInputDto);
            product.RegistrationDate = DateTime.Now;

            _repository.Create(product);

            var productViewDto = _mapper.Map<ProductViewDto>(product);

            //Retorna 201 Created, inclui no cabeçalho da resposta a URL do recurso e envia o produto no body da response
            return new CreatedAtRouteResult("GetProduct", new { id = productViewDto.Id }, productViewDto);
        }


        [HttpPut("{id:int}")]
        public ActionResult<ProductViewDto> Put(int id, ProductInputDto productInputDto)
        {
            var existingProduct = _repository.GetById(p => p.Id == id);

            if (existingProduct is null)
            {
                return NotFound("Produto não encontrado");
            }

            var product = _mapper.Map(productInputDto, existingProduct);

            _repository.Update(product);

            var productViewDto = _mapper.Map<ProductViewDto>(product);

            return Ok(productViewDto); //No parâmetro do método Ok se coloca o vai ser mostrado na response
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


        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProductViewDto> Patch(int id, JsonPatchDocument<ProductInputPatchDto> patchDocument) //JsonPatchDocument<Modelo> define que o corpo da requisição receberá um documento patch 
        {
            if (patchDocument == null)
            {
                return BadRequest("Corpo do documento patch vazio");
            }

            var product = _repository.GetById(p => p.Id == id);

            if (product is null) 
            {
                return NotFound("Produto não encontrado");
            }

            var productToPatch = _mapper.Map<ProductInputPatchDto>(product); //Passa as propriedades de product para ProductInputPatchDto

            patchDocument.ApplyTo(productToPatch, ModelState); //Esse comando aplica as modificações que vieram de um JSON Patch (um tipo de requisição parcial) sobre o objeto productToPatch

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(productToPatch, product); //Passa as propriedades de productToPatch para o produto que será atualizado no banco de dados
            _repository.Update(product);

            var productViewDto = _mapper.Map<ProductViewDto>(product);

            return Ok(productViewDto);
        }
    }
}
