using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Dtos;

namespace Jogos.Service.Application.Interface
{
    public interface IuseCaseJogos
    {
        JogosResponse GetAllJogos();
        JogosResponse Create(JogosRequest request);
    }
}
