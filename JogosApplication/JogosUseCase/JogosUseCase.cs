using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;

namespace Jogos.Service.Application.JogosUseCase
{
    public class JogosUseCase : IuseCaseJogos
    {
        private IuseCaseJogos _useCaseJogos;

        public JogosUseCase(IuseCaseJogos jogos)
        {
            _useCaseJogos = jogos;
        }
        public JogosResponse Create(JogosRequest request)
        {
            throw new NotImplementedException();
        }

        public JogosResponse GetAllJogos()
        {
            throw new NotImplementedException();
        }
    }
}
