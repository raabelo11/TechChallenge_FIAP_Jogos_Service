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
        public List<Jogo> Get()
        {
            return _useCaseJogos.GetAllJogos();
        }

        // GET api/<JogosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JogosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JogosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JogosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
