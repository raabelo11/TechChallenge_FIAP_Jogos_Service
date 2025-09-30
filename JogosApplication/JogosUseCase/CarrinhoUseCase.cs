using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Enums;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Jogos.Service.Application.JogosUseCase
{
    public class CarrinhoUseCase : ICarrinho
    {
        private readonly IPagamentoClient _pagamentoClient;
        private readonly IPedidoJogo _pedido;
        private readonly IJogo _jogo;
        private readonly IBiblioteca _biblioteca;
        private readonly ILogger<CarrinhoUseCase> _logger;
        public CarrinhoUseCase(IPagamentoClient pagamentoClient, IPedidoJogo jogosRepository, IJogo jogo, IBiblioteca biblioteca,ILogger<CarrinhoUseCase> logger)
        {
            _pagamentoClient = pagamentoClient;
            _pedido = jogosRepository;
            _jogo = jogo;
            _biblioteca = biblioteca;
            _logger = logger;
        }
        public async Task <JogosResponse> Processar(ProcessamentoRequest processamentoRequest)
        {
            try
            {
                var jogo = _jogo.Listar().Find(x => x.Id == processamentoRequest.IdJogo);                
                if(jogo is null)
                {
                    _logger.LogWarning("Jogo não localizado. IdJogo: {IdJogo}", processamentoRequest.IdJogo);
                    return new JogosResponse
                    {
                        Errors = new string[] { "Jogo não foi possível localizar" }
                    };
                }

                PedidoJogo pedidoJogo = new PedidoJogo
                {
                    IdJogo = processamentoRequest.IdJogo,
                    IdCliente = processamentoRequest.IdCliente
                };
                _logger.LogInformation("Iniciando processamento do pedido. IdCliente: {IdCliente}, IdJogo: {IdJogo}", processamentoRequest.IdCliente, processamentoRequest.IdJogo);
                var ReqPagamento = await _pagamentoClient.IncluirJogo(pedidoJogo);

                if (ReqPagamento.Ok)
                {
                    _logger.LogInformation("Pedido processado com sucesso. IdCliente: {IdCliente}, IdJogo: {IdJogo}", processamentoRequest.IdCliente, processamentoRequest.IdJogo);
                    _pedido.Adicionar(pedidoJogo);
                    return ReqPagamento;
                }
                _logger.LogInformation("Erro ao processar pedido. IdCliente: {IdCliente}, IdJogo: {IdJogo}", processamentoRequest.IdCliente, processamentoRequest.IdJogo);
                return new JogosResponse
                {
                 
                    Errors = new string[] { "Erro ao processar pedido" }
                };

            }
            catch (Exception ex)
            {
                return new JogosResponse
                {
                    Ok = false,
                    Errors = new string[] { ex.Message }
                };

            }
        }
        public async Task<JogosResponse> Confirmar(ConfirmarPedidoDTO confirmarPedidoDTO)
        {
            _logger.LogInformation("Iniciando confirmação do pedido. HashPedido: {HashPedido}, Status: {Status}", confirmarPedidoDTO.HashPedido, confirmarPedidoDTO.status);
            var validateStatus = Enum.IsDefined(typeof(StatusProcessamento), confirmarPedidoDTO.status);
            var pedido = _pedido.Listar().Find(x => x.HashPedido == confirmarPedidoDTO.HashPedido);

            if (validateStatus is false || pedido is null)
            {
                _logger.LogWarning("Hash ou status não localizado. HashPedido: {HashPedido}, Status: {Status}", confirmarPedidoDTO.HashPedido, confirmarPedidoDTO.status);
                return new JogosResponse
                {
                    Errors = new string[] { "Hash ou status não localizado" },
                    Ok = false
                };
            }
            switch (confirmarPedidoDTO.status)
            {   

                case 2:
                    pedido.Status = StatusProcessamento.Aprovado;
                    _pedido.Atualizar(pedido);
                    var biblioteca = new Biblioteca(pedido.IdCliente,pedido.IdJogo);
                    _logger.LogInformation("Pedido aprovado e jogo adicionado à biblioteca. IdCliente: {IdCliente}, IdJogo: {IdJogo}", pedido.IdCliente, pedido.IdJogo);
                    _biblioteca.Adicionar(biblioteca);
                    break;
                case 3:
                    _logger.LogInformation("Pedido cancelado. HashPedido: {HashPedido}", confirmarPedidoDTO.HashPedido);
                    pedido.Status = StatusProcessamento.Cancelado;
                    _pedido.Atualizar(pedido);
                    break;
            }
            return new JogosResponse
            {
                Ok = true,
                data = pedido
            };
        }

        public async Task<JogosResponse> ListarBiblioteca(int idCliente)
        {
            _logger.LogInformation(idCliente, "Listando biblioteca do cliente. IdCliente: {IdCliente}", idCliente);
            var bibliotecaJogos = await _biblioteca.ListarBiblioteca(idCliente);
            var jogos = bibliotecaJogos.Select(x => new
            {
                nome = x.Nome,
                descricao = x.Descricao,
                genero = x.Genero.ToString(),
                Estudio = x.Estudio.ToString()
            });
            return new JogosResponse
            {
                Ok = true,
                data = jogos
            };
        }
    }
}
