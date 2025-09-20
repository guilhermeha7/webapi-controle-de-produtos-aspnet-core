using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ProdutosApi.Models
{
    public class Category
    {
        public int Id { get; set; } // EF identifica como Primary Key por convenção

        [Required] //Define o atributo como obrigatório
        [StringLength(80)] //Define o tamanho máximo de caracteres que Name pode armazenar
        public string Name { get; set; }

        [Required] 
        [StringLength(2000)] 
        public string ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new Collection<Product>(); 
        }
    }
}
