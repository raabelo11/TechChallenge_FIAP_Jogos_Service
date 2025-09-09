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
        [Key]
        public int Id { get; set; }
        public double Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        [Required(ErrorMessage = "O campo Estudio é obrigatório")]
        public Estudio Estudio { get; set; }
        [Required(ErrorMessage = "O campo Genero é obrigatório")]
        public Genero Genero { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [StringLength(200)]
        public string Descricao { get; set; }
    }
}
