
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Infrastructure.Queue
{
    public interface IRabbitMQClient
    {
        Task FilaProcessamento(PedidoJogo pedidoJogo);
    }
}
