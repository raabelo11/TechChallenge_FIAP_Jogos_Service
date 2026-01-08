
using System.Data.Common;
using AutoMapper;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Enums;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Microsoft.Extensions.Logging;
using Pagamentos.Service.Application.Dtos;

namespace Jogos.Service.Application.JogosUseCase
{
    public class BibliotecaUseCase(ILogger<Biblioteca> logger, IPedidoJogo pedidoJogo, IBiblioteca biblioteca, IPedidoEvent pedidoEvent,IMapper mapper) : IBibliotecaUseCase
    {
        private readonly ILogger<Biblioteca> _logger = logger;
        private readonly IPedidoJogo _pedidoJogo = pedidoJogo;
        private readonly IBiblioteca _biblioteca = biblioteca;
        private readonly IPedidoEvent _pedidoEvent = pedidoEvent;
        private readonly IMapper _mapper = mapper;
        public async Task SalvarJogoBiblioteca(BibliotecaQueue bibliotecaQueue)
        {
            _logger.LogInformation($"Iniciando o salvamento do jogo na biblioteca do pedido: {bibliotecaQueue.HashPedido}");
            var validateStatus = Enum.IsDefined(typeof(StatusProcessamento), bibliotecaQueue.status);
            try
            {
                var ListaPedidos = await _pedidoJogo.Listar();
                var pedido = ListaPedidos.Find(x => x.HashPedido == bibliotecaQueue.HashPedido);
                await _pedidoEvent.Adicionar(new PedidoEvent
                {
                    DataEvento = DateTime.UtcNow,
                    HashPedido = bibliotecaQueue.HashPedido,
                    EstadoPedido = $"O Pedido foi alterado para o seguinte status: {bibliotecaQueue.status.ToString()}"
                });
                switch (bibliotecaQueue.status)
                {
                    case 2:
                        pedido.Status = StatusProcessamento.Aprovado;
                        await _pedidoJogo.Atualizar(pedido);
                        Biblioteca biblioteca = _mapper.Map<Biblioteca>(pedido);
                        await _biblioteca.Adicionar(biblioteca);
                        _logger.LogInformation($"O pedido foi aprovado: {pedido.HashPedido}");

                        break;
                    case 3:
                        _logger.LogInformation($"O pedido foi cancelado: {pedido.HashPedido}");
                        pedido.Status = StatusProcessamento.Cancelado;
                        await _pedidoJogo.Atualizar(pedido);
                        break;
                }

            }
            catch (DbException ex)
            {
                _logger.LogError($"Erro ao realizar operação no banco de dados {bibliotecaQueue.HashPedido}, Erro: {ex.Message}");
                throw;
            }

        }

    }
}
