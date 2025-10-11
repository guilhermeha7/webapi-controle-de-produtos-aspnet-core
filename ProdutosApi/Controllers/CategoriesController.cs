using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProdutosApi.Context;
using ProdutosApi.DTOs;
using ProdutosApi.Models;
using ProdutosApi.Pagination;
using ProdutosApi.Repositories;

namespace ProdutosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;


        public CategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<CategoryViewDto>> Get()
        {
            var categories = _repository.GetAll();

            var categoriesViewDto = _mapper.Map<IEnumerable<CategoryViewDto>>(categories);

            return Ok(categoriesViewDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoryViewDto>> Get([FromQuery] PaginationParameters paginationParams)
        {
            var categories = _repository.GetWithPagination(paginationParams, c => c.Id);

            int currentPage = paginationParams.PageNumber;
            int pageSize = paginationParams.PageSize;
            int totalItems = _repository.GetTotalItems();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            PaginationHeaders paginationHeaders = new PaginationHeaders(currentPage, totalPages, pageSize, totalItems);
        
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationHeaders));

            var categoriesViewDto = _mapper.Map<IEnumerable<CategoryViewDto>>(categories);
            return Ok(categoriesViewDto);
        }


        [HttpGet("products")]
        public ActionResult<IEnumerable<CategoryViewDto>> GetCategoriesWithProducts()
        {
            var categories = _repository.GetCategoriesWithProducts();

            var categoriesViewDto = _mapper.Map<IEnumerable<CategoryViewDto>>(categories);

            return Ok(categoriesViewDto);
        }


        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<CategoryViewDto> Get(int id)
        {
            Category category = _repository.GetByIdAsNoTracking(c => c.Id == id);
            
            if (category is null)
            {
                NotFound("Categoria não encontrada");
            }

            var categoryViewDto = _mapper.Map<CategoryViewDto>(category);

            return Ok(categoryViewDto);
        }


        [HttpPost]
        public ActionResult<CategoryViewDto> Post(CategoryInputDto categoryInputDto)
        {
            if (categoryInputDto is null)
            {
                return BadRequest("Não é possível cadastrar uma categoria vazia");
            }

            var category = _mapper.Map<Category>(categoryInputDto);

            _repository.Create(category);

            var categoryViewDto = _mapper.Map<CategoryViewDto>(category);

            return new CreatedAtRouteResult("GetCategory", new { id = categoryViewDto.Id }, categoryViewDto);
        }


        [HttpPut("{id:int}")]
        public ActionResult<CategoryViewDto> Put(int id, CategoryInputDto categoryInputDto)
        {
            var existingCategory = _repository.GetById(c => c.Id == id);

            if (existingCategory is null)
            {
                return NotFound("Categoria não encontrada");
            }

            _mapper.Map(categoryInputDto, existingCategory);

            _repository.Update(existingCategory);

            var categoryViewDto = _mapper.Map<CategoryViewDto>(existingCategory);

            return Ok(categoryViewDto);
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _repository.GetById(c => c.Id == id);

            if (category is null)
            {
                return NotFound("Categoria não encontrada");
            }

            _repository.Delete(category);

            return Ok();
        }
    }
}
