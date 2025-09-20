using System.Collections.ObjectModel;

namespace ProdutosApi.Models
{
    public class Category
    {
        public int Id { get; set; } // EF identifica como Primary Key por convenção
        public string Name { get; set; }
        public int ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new Collection<Product>(); 
        }
    }
}
