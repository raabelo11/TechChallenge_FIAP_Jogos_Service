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
        public ICollection<Biblioteca>? Bibliotecas { get; set; }
    }
}
