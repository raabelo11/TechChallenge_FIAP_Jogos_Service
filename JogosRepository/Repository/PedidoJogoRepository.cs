using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Context;

namespace Jogos.Service.Infrastructure.Repository
{
    public class PedidoJogoRepository : RepositoryGeneric<PedidoJogo>, IPedidoJogo
    {
        public PedidoJogoRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
