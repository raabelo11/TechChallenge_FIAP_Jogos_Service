using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;

namespace Jogos.Service.Application.Interface
{
    public interface ICarrinho
    {
        Task <JogosResponse>Processar(ProcessamentoRequest processamentoRequest);
        Task<JogosResponse> Confirmar(ConfirmarPedidoDTO confirmarPedidoDTO);
        Task<JogosResponse> ListarBiblioteca(int idCliente);
    }
}
