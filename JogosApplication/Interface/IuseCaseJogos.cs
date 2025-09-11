using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Application.Interface
{
    public interface IuseCaseJogos
    {
        List<Jogo> GetAllJogos();
        JogosResponse Create(JogosRequest request);
    }
}
