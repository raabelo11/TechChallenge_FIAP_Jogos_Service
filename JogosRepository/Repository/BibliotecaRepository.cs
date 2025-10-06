using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Service.Infrastructure.Repository
{
    public class BibliotecaRepository : RepositoryGeneric<Biblioteca>, IBiblioteca
    {
        private readonly ApplicationDbContext _context;
        public BibliotecaRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<Jogo>> ListarBiblioteca(int idCliente)
        {
            var biblioteca = await _context.Bibliotecas.Where(x => x.IdCliente.Equals(idCliente)).Select(j => j.Jogo).ToListAsync();
            return biblioteca;
        }

    }
}
