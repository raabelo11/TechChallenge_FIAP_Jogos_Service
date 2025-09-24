using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jogos.Service.Application.Dtos
{
    public class ProcessamentoRequest
    {
        public int IdJogo { get; set; }
        public int IdCliente { get; set; }
    }
}
