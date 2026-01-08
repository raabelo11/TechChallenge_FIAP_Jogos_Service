using MassTransit;

namespace Pagamentos.Service.Application.Dtos
{
    [EntityName("biblioteca-fila")]
    public class BibliotecaQueue
    {
        public Guid HashPedido { get; set; }
        public int status { get; set; }
    }
}
