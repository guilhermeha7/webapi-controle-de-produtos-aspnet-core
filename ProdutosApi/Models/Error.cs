using System.Text.Json;

namespace ProdutosApi.Models
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; } //Sequência de chamadas de métodos que levou até o erro/exceção. É o “rastro” do que o programa estava executando. Ajuda a identificar onde a falha aconteceu. Não se deve usar em produção por segurança.

        public override string ToString()
        {
            return JsonSerializer.Serialize(this); //Converte o objeto para JSON
        }
    }
}
