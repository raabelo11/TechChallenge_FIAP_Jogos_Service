using System;
using MassTransit;

namespace Jogos.Service.Infrastructure.Queue.ModelQueue
{
    [EntityName("pedido-jogo")]
    public class PedidoJogoQueue
    {
        public Guid HashPedido { get; set; } = new Guid(Guid.NewGuid().ToString());
        public int IdJogo { get; set; }
        public int IdCliente { get; set; }
        // Usando int para compatibilidade com o consumer
        // 1 = Pendente, 2 = Aprovado, 3 = Cancelado
        public int Status { get; set; } = 1; // 1 = Pendente
    }
}
