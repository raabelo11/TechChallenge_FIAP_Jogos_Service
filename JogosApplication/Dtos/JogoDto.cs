using Jogos.Service.Domain.Enums;

namespace Jogos.Service.Application.Dtos
{
    public class JogoDto
    {
        public double Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public Estudio Estudio { get; set; }
        public Genero Genero { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
