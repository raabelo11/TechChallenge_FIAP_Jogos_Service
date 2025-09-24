using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;

namespace Jogos.Service.Application.Interface
{
    public interface IClient
    {
        Task <JogosResponse>Processar(ProcessamentoRequest processamentoRequest);
        Task<JogosResponse> Confirmar(int idCliente, int idJogo, int status);
    }
}
