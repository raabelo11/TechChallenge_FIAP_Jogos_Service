using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogos.Service.Domain.Models
{
    public class Biblioteca
    {
        public int IdCliente { get; set; }
        public List<Jogo> jogos { get; set; } = new List<Jogo>();

    }
}
