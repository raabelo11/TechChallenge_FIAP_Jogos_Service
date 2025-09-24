using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Enums;

namespace Jogos.Service.Domain.Models
{
    public class PedidoJogo
    {
        public Guid HashPedido { get; set; } = new Guid(Guid.NewGuid().ToString());
        public int IdJogo { get; set; }
        public int IdCliente { get; set; }
        public StatusProcessamento Status { get; set; } = StatusProcessamento.Pendente;
    }
}
