using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogos.Service.Domain.Models
{
    public class PedidoEvent
    {
      public int Id { get; set; }
      public Guid HashPedido { get; set; }
      public string EstadoPedido { get; set; }
      public DateTime DataEvento { get; set; }

    }
}
