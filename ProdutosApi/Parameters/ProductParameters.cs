using ProdutosApi.Parameters.Enums;

namespace ProdutosApi.Parameters
{
    public class ProductParameters : PaginationParameters
    {
        /*O usuário vai conseguir filtrar por Nome, Preço Mínimo e Preço Máximo.
         O usuário vai conseguir ordenar por Nome, Preço e Data de registro.*/
        
        //Parâmetros de Filtro
        public string? Name { get; set; } = null;
        public decimal? MinPrice { get; set; } = 0;
        public decimal? MaxPrice { get; set; } = 1000000;

        //Parâmetros de Ordenação
        public ProductSort SortBy { get; set; } = ProductSort.RegistrationDate;
        public SortDirection SortDirection { get; set; } = SortDirection.Desc;
    }
}
