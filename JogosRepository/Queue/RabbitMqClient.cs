using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Queue.ModelQueue;
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
            // Converte o modelo de domínio para o DTO de fila que tem o atributo [EntityName]
            // Converte o enum StatusProcessamento para int para compatibilidade com o consumer
            var pedidoJogoQueue = new PedidoJogoQueue
            {
                HashPedido = pedidoJogo.HashPedido,
                IdJogo = pedidoJogo.IdJogo,
                IdCliente = pedidoJogo.IdCliente,
                Status = (int)pedidoJogo.Status // Converte enum para int
            };
            
            await _bus.Publish(pedidoJogoQueue);
        }
    }
}
