using ProdutosApi.Models;
using ProdutosApi.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProdutosApi.DTOs
{
    public class ProductInputDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80)]
        [FirstLetterCapitalized] //Validação personalizada
        public string Name { get; set; }

        [Required]
        [StringLength(800)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(2000)]
        public string ImageUrl { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
