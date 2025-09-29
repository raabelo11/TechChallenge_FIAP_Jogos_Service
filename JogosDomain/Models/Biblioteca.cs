using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogos.Service.Domain.Models
{
    public class Biblioteca
    {
        public Biblioteca(int ID,int Jogo)
        {
            IdCliente = ID;
            IdJogo = Jogo;
        }
        public int IdCliente { get; set; }
        public int IdJogo { get; set; }
        public Jogo Jogo { get; set; }

    }
}
