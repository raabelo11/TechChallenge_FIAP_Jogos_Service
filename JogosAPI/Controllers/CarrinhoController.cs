using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jogos.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly IClient _client;
        public CarrinhoController(IClient client)
        {
            _client = client;
        }
        [HttpPost("CriarPedido")]
        public async Task<ActionResult<bool>> Post([FromBody] ProcessamentoRequest processamentoRequest)
        {
            var adicionarCarrinho = await _client.Processar(processamentoRequest);
            return adicionarCarrinho.Ok ? Ok(adicionarCarrinho.Ok) : BadRequest(adicionarCarrinho.Ok);
        }
        [HttpPost("ConfirmarPedido")]
        public async Task<ActionResult<bool>> ConfirmarPedido(ConfirmarPedidoDTO confirmarPedidoDTO)
        {

        }
    }
}
