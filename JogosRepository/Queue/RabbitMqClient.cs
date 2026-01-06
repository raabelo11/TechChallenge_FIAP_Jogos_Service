using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using MassTransit;

namespace Jogos.Service.Infrastructure.Queue
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IBus _bus;
        public RabbitMqClient(IBus bus)
        {
            _bus = bus;
        }
        public async Task FilaProcessamento(PedidoJogo pedidoJogo)
        {
            await _bus.Publish(pedidoJogo);
        }
    }
}
