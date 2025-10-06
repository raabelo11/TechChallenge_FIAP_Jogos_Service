using System.Text.Json.Serialization;

namespace Jogos.Service.Application.Dtos
{
    public class ProcessamentoRequest
    {
        public int IdJogo { get; set; }
        [JsonIgnore]
        public int IdCliente { get; set; }
    }
}
