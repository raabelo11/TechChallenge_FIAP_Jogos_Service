
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Infrastructure.Queue
{
    internal interface IRabbitMQClient
    {
        void FilaProcessamento(PedidoJogo pedidoJogo);
    }
}
