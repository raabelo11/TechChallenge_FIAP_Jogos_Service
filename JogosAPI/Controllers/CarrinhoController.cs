using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Jogos.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinho _carrinho;
        public CarrinhoController(ICarrinho client)
        {
            _carrinho = client;
        }
        [HttpPost("CriarPedido")]
        public async Task<ActionResult<bool>> Post([FromBody] ProcessamentoRequest processamentoRequest)
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is int userId)
            {
                processamentoRequest.IdCliente = userId;
                var adicionarCarrinho = await _carrinho.Processar(processamentoRequest);
                return Ok(adicionarCarrinho.Ok);
            }
            return Unauthorized();
        }
        [HttpPost("ConfirmarPedido")]
        public async Task<ActionResult<bool>> ConfirmarPedido([FromBody] ConfirmarPedidoDTO confirmarPedidoDTO)
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is int userId)
            {
                var atualizarPedido = await _carrinho.Confirmar(confirmarPedidoDTO);
                return Ok(atualizarPedido.Ok);
            }

            return Unauthorized();
        }
        [HttpGet("ListarBibliotecaCliente")]
        public async Task<ActionResult<bool>> ListarBibliotecaClinte(int id)
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is int userId)
                id = userId;

            var listarBiblioteca = await _carrinho.ListarBiblioteca(id);
            return listarBiblioteca.Ok ? Ok(listarBiblioteca.data) : BadRequest(listarBiblioteca.Errors);
        }

        [HttpGet("ListarIndicações")]
        public async Task<ActionResult<bool>> ListarIndicações(int id)
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is int userId)
                id = userId;

            var listarBiblioteca = await _carrinho.ListarJogosPorEstudioPreferido(id);
            return listarBiblioteca.Ok ? Ok(listarBiblioteca.data) : BadRequest(listarBiblioteca.Errors);
        }
    }
}
