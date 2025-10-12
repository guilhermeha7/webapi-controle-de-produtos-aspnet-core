namespace ProdutosApi.Parameters
{
    public class PaginationParameters
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1; //= 1; Atribui um valor inicial à propriedade PageNumber
        private int _pageSize = maxPageSize; 
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value; //(condição) ? valorSeVerdadeiro : valorSeFalso
            }
        }
    }
}
