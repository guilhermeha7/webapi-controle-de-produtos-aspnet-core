using ProdutosApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ProdutosApi.DTOs
{
    public class CategoryViewDto
    {
        public int Id { get; set; }

        [Required] //Define o atributo como obrigatório
        [StringLength(80)] //Define o tamanho máximo de caracteres que Name pode armazenar
        public string Name { get; set; }

        [Required]
        [StringLength(2000)]
        public string ImageUrl { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
