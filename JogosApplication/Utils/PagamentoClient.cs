using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Application.Utils
{
    public class PagamentoClient(HttpClient httpClient) : IPagamentoClient
    {
        private readonly HttpClient _httpClient = httpClient;   
        public async Task<JogosResponse> IncluirJogo(PedidoJogo jogo)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(jogo);
            var response = await _httpClient.PostAsync("api/Pagamento/receberPedido", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return new JogosResponse
                {
                    Ok = true,
                    data = response.Content.ReadFromJsonAsync<Object>().Result
                };
            }
            else
            {
                return new JogosResponse
                {
                    Ok = false,
                    Errors = new string[] { "Erro ao receber pedido" }
                };
            }
        } 
    }
}
