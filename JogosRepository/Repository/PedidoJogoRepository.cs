using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
