using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Enums;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;

namespace Jogos.Service.Application.JogosUseCase
{
    public class ClientAPI : IClient
    {
        private readonly HttpClient _httpClient;
        private readonly IPedidoJogo _pedido;
        private readonly IJogo _jogo;
        public ClientAPI(HttpClient htpp, IPedidoJogo jogosRepository, IJogo jogo)
        {
            _httpClient = htpp;
            _pedido = jogosRepository;
            _jogo = jogo;
        }
        public async Task <JogosResponse> Processar(ProcessamentoRequest processamentoRequest)
        {
            try
            {
                var jogo = _jogo.Listar().Find(x => x.Id == processamentoRequest.IdJogo);
                if(jogo is null)
                {
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
                var adicionar = _pedido.Adicionar(pedidoJogo);
                var json = System.Text.Json.JsonSerializer.Serialize(pedidoJogo);
                var response = await _httpClient.PostAsync("api/Pagamento/receberPedido", new StringContent(json, Encoding.UTF8, "application/json"));
                if(response.IsSuccessStatusCode)
                {
                   
                    return new JogosResponse
                    {
                        Ok = true,
                        data = response.Content.ReadFromJsonAsync<object>().Result
                    };
                }else
                {
                    return new JogosResponse
                    {
                        Ok = false,
                        Errors = new string[] { "Erro ao receber pedido" }
                    };
                }
                
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
        public async Task<JogosResponse> Confirmar(int idCliente, int idJogo, int status)
        {
            var validateStatus = Enum.IsDefined(typeof(StatusProcessamento), status);
            var JogoLocation = _pedido.Listar().Find(x => x.IdJogo == idJogo);
            if (validateStatus is false || JogoLocation is null)
            {
                return new JogosResponse
                {
                    Errors = new string[] { "Id de jogo ou status não localizado" }
                };
            }
        }
    }
}
