using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pagamentos.Service.Application.Dtos;

namespace Jogos.Service.Application.Interface
{
    public interface IBibliotecaUseCase
    {
       public Task SalvarJogoBiblioteca(BibliotecaQueue bibliotecaQueue);
    }
}
