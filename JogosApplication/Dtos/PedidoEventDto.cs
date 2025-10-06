
namespace Jogos.Service.Application.Dtos
{
    public class PedidoEventDto
    {
        public Guid HashPedido { get; set; }
        public string EstadoPedido { get; set; }
        public DateTime DataEvento { get; set; }
    }
}
