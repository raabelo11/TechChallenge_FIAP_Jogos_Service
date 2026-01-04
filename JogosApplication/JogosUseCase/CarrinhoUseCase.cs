using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using AutoMapper;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Application.Utils;
using Jogos.Service.Domain.Enums;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Queue;
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
        private readonly ElasticClient _elastic;
        private readonly IPedidoEvent _pedidoEvent;
        private readonly IMapper _mapper;
        private readonly IRabbitMQClient _rabbitMQClient;
        public CarrinhoUseCase(IPagamentoClient pagamentoClient, IPedidoJogo jogosRepository, IJogo jogo, IBiblioteca biblioteca,ILogger<CarrinhoUseCase> logger, ElasticClient elastic, IPedidoEvent pedidoEvent, IMapper mapper, IRabbitMQClient rabbitMQClient)
        {
            _pagamentoClient = pagamentoClient;
            _pedido = jogosRepository;
            _jogo = jogo;
            _biblioteca = biblioteca;
            _logger = logger;
            _elastic = elastic;
            _pedidoEvent = pedidoEvent;
            _mapper = mapper;
            _rabbitMQClient = rabbitMQClient;
        }
        public async Task <JogosResponse> Processar(ProcessamentoRequest processamentoRequest)
        {
            try
            {
                var jogo = _jogo.Listar().Result.Find(x => x.Id == processamentoRequest.IdJogo);                
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
                await _rabbitMQClient.FilaProcessamento(pedidoJogo);
                var ReqPagamento = await _pagamentoClient.IncluirJogo(pedidoJogo);

                if (ReqPagamento.Ok)
                {

                    _logger.LogInformation("Pedido processado com sucesso. IdCliente: {IdCliente}, IdJogo: {IdJogo}", processamentoRequest.IdCliente, processamentoRequest.IdJogo);
                    PedidoEventDto pedidoEvent = new PedidoEventDto();
                    pedidoEvent.DataEvento = DateTime.UtcNow;
                    pedidoEvent.HashPedido = pedidoJogo.HashPedido;
                    pedidoEvent.EstadoPedido = $"O Pedido foi salvo com o seguinte status: {pedidoJogo.Status.ToString()}";
                    var PedidoMap = _mapper.Map<PedidoEvent>(pedidoEvent);
                    await _pedidoEvent.Adicionar(PedidoMap);

                    await _pedido.Adicionar(pedidoJogo);
                    await _elastic.SalvarJogo(jogo.Nome, jogo.Id);
                    await _elastic.salvarPreferencia(((int)jogo.Estudio), processamentoRequest.IdCliente);
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
            var pedido = _pedido.Listar().Result.Find(x => x.HashPedido == confirmarPedidoDTO.HashPedido);

            if (validateStatus is false || pedido is null)
            {
                _logger.LogWarning("Hash ou status não localizado. HashPedido: {HashPedido}, Status: {Status}", confirmarPedidoDTO.HashPedido, confirmarPedidoDTO.status);
                return new JogosResponse
                {
                    Errors = new string[] { "Hash ou status não localizado" },
                    Ok = false
                };
            }
            PedidoEventDto pedidoEvent = new PedidoEventDto();
            pedidoEvent.DataEvento = DateTime.UtcNow;
            pedidoEvent.HashPedido = confirmarPedidoDTO.HashPedido;
            pedidoEvent.EstadoPedido = $"O Pedido foi alterado para o seguinte status: {confirmarPedidoDTO.status.ToString()}";
            var PedidoMap = _mapper.Map<PedidoEvent>(pedidoEvent);
            await _pedidoEvent.Adicionar(PedidoMap);
            switch (confirmarPedidoDTO.status)
            {   

                case 2:
                    pedido.Status = StatusProcessamento.Aprovado;
                    await _pedido.Atualizar(pedido);
                    Biblioteca biblioteca = new Biblioteca();
                    biblioteca.IdJogo = pedido.IdJogo;
                    biblioteca.IdCliente = pedido.IdCliente;
                    _logger.LogInformation("Pedido aprovado e jogo adicionado à biblioteca. IdCliente: {IdCliente}, IdJogo: {IdJogo}", pedido.IdCliente, pedido.IdJogo);
                    await _biblioteca.Adicionar(biblioteca);
                    break;
                case 3:
                    _logger.LogInformation("Pedido cancelado. HashPedido: {HashPedido}", confirmarPedidoDTO.HashPedido);
                    pedido.Status = StatusProcessamento.Cancelado;
                    await _pedido.Atualizar(pedido);
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

        public async Task<JogosResponse> ListarJogosPorEstudioPreferido(int idCliente)
        {
            var response = await _elastic.EstudioPreferido(idCliente);
            var jogos = _jogo.Listar().Result.FindAll(x => (int)x.Estudio == response);
            return new JogosResponse
            {
                Ok = true,
                data = jogos
            };
        }
    }
}
