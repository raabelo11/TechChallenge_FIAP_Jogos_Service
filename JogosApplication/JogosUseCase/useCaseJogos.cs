using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Application.JogosUseCase
{
    public class useCaseJogos : IuseCaseJogos
    {
        private readonly IJogos _jogos;
        public useCaseJogos(IJogos jogos)
        {
            _jogos = jogos;
        }
        public JogosResponse Create(JogosRequest request)
        {
            throw new NotImplementedException();
        }

        public List<Jogo> GetAllJogos()
        {
            return _jogos.Listar();
        }
    }
}
