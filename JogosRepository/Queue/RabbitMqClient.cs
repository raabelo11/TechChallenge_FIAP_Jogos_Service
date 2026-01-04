using Jogos.Service.Domain.Models;
using MassTransit;

namespace Jogos.Service.Infrastructure.Queue
{
    public class RabbitMqClient : IRabbitMQClient
    {
        private readonly IBus _bus;
        public RabbitMqClient(IBus bus)
        {
            _bus = bus;
        }
        public async Task FilaProcessamento(PedidoJogo pedidoJogo)
        {
            var fila = new Uri("queue:carrinho");
            var sendEndpoint = await _bus.GetSendEndpoint(fila);

            await sendEndpoint.Send<PedidoJogo>(pedidoJogo);
        }
    }
}
