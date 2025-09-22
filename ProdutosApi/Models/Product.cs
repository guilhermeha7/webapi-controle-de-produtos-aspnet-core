using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdutosApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [Required]
        [StringLength(800)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")] //Define o tipo da coluna no banco de dados como decimal, podendo armazenar valores de -9999999999999.99 até 9999999999999.99.
        public decimal Price { get; set; }

        [Required]
        [StringLength(2000)]
        public string ImageUrl { get; set; }

        public int Stock { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Category? Category { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
