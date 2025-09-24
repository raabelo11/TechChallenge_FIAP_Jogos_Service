using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Domain.Interface
{
    public interface IBiblioteca
    {
        bool Adicionar(Biblioteca biblioteca);
        List<Biblioteca> Listar(int id);
    }
}
