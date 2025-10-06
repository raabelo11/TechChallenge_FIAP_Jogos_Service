using Jogos.Service.Domain.Models;

namespace Jogos.Service.Domain.Interface
{
    public interface IBiblioteca : IRepositoryGeneric<Biblioteca>
    {
        Task<ICollection<Jogo>> ListarBiblioteca(int idCliente);
    }
}
