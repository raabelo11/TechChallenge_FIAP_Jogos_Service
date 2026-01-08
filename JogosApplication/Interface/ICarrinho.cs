
using Jogos.Service.Application.Dtos;

namespace Jogos.Service.Application.Interface
{
    public interface ICarrinho
    {
        Task <JogosResponse>Processar(ProcessamentoRequest processamentoRequest);
        Task<JogosResponse> VerificarStatusPedido(ConfirmarPedidoDTO confirmarPedidoDTO);
        Task<JogosResponse> ListarBiblioteca(int idCliente);
        Task<JogosResponse> ListarJogosPorEstudioPreferido(int idCliente);

    }
}
