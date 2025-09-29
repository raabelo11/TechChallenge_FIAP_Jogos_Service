using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogos.Service.Application.Dtos
{
    public class ConfirmarPedidoDTO
    {
        public Guid HashPedido { get; set; }
        public int status { get; set; }
    }
}
