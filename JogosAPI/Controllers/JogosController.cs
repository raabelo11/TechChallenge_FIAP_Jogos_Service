using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jogos.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IuseCaseJogos _useCaseJogos;

        public JogosController(IuseCaseJogos iuseCaseJogos)
        {
            _useCaseJogos = iuseCaseJogos;
        }
        // GET: api/<JogosController>
        [HttpGet]
        [Route("ListarJogos")]
        public async Task <JogosResponse> Get()
        {
            var header = HttpContext.Items["UserId"];
            ; // Apenas para evitar o warning de variável não utilizada
            return _useCaseJogos.listarJogos();
        }
        [HttpGet]
        [Route("ListarCategorias")]
        public async Task<JogosResponse> GetCategorias()
        {
            return _useCaseJogos.listarCategorias();
        }
        [HttpGet]
        [Route("ListarEstudios")]
        public async Task<JogosResponse> GetEstudios()
        {
            return _useCaseJogos.listarEstudios();
        }
        [Route("CadastrarJogos")]
        [HttpPost]
        public ActionResult<JogosResponse> Post([FromBody] JogoDto jogoDto)
        {
           var jogoCriado = _useCaseJogos.Create(jogoDto);
            return jogoCriado.Ok ? Ok(jogoCriado) : BadRequest(jogoCriado);
        }

        // PUT api/<JogosController>/5
        [Route("AtualizarJogos")]
        [HttpPatch]
        public ActionResult<JogosResponse> Patch([FromBody] JogoRequest jogo)
        {
            var jogoAtualizado = _useCaseJogos.Update(jogo);
            return jogoAtualizado.Ok ? Ok(jogoAtualizado) : BadRequest(jogoAtualizado);
        }
    }
}
