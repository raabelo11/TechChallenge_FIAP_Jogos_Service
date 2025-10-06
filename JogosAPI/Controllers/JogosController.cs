using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
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
            return await _useCaseJogos.listarJogos();
        }
        [HttpGet("Recomendacoes")]
        public async Task<JogosResponse> GetRecomendacoes()
        {
            return await _useCaseJogos.ListarRecomendacoes();
        }
        [HttpGet]
        [Route("ListarCategorias")]
        public JogosResponse GetCategorias()
        {
            return  _useCaseJogos.listarCategorias();
        }
        [HttpGet]
        [Route("ListarEstudios")]
        public JogosResponse GetEstudios()
        {
            return  _useCaseJogos.listarEstudios();
        }
        [Route("CadastrarJogos")]
        [HttpPost]
        public async Task<ActionResult<JogosResponse>> Post([FromBody] JogoDto jogoDto)
        {
           var jogoCriado =  await _useCaseJogos.Create(jogoDto);
            return jogoCriado.Ok ? Ok(jogoCriado) : BadRequest(jogoCriado);
        }

        // PUT api/<JogosController>/5
        [Route("AtualizarJogos")]
        [HttpPatch]
        public async Task<ActionResult<JogosResponse>> Patch([FromBody] JogoRequest jogo)
        {
            var jogoAtualizado = await _useCaseJogos.Update(jogo);
            return jogoAtualizado.Ok ? Ok(jogoAtualizado) : BadRequest(jogoAtualizado);
        }
    }
}
