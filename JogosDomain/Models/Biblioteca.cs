namespace Jogos.Service.Domain.Models
{
    public class Biblioteca
    {
        public int IdCliente { get; set; }
        public int IdJogo { get; set; }
        public Jogo Jogo { get; set; }

    }
}
