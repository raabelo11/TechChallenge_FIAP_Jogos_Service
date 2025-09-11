using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Enums;

namespace Jogos.Service.Domain.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public double Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public Estudio Estudio { get; set; }
        public Genero Genero { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
