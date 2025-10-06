using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Context;

namespace Jogos.Service.Infrastructure.Repository
{
    public class JogoRepository : RepositoryGeneric<Jogo>, IJogo
    {
        public JogoRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
