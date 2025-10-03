using ProdutosApi.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProdutosApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage="O nome é obrigatório")]
        [StringLength(80)]
        [FirstLetterCapitalized] //Validação personalizada
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

        [Required]
        public int Stock { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore] //Esse atributo faz com que a propriedade não seja convertida para JSON na serialização (transformação do objeto C# para JSON) 
        public Category? Category { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
