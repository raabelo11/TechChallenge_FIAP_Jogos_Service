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
    public class PedidoEventRepository : RepositoryGeneric<PedidoEvent>, IPedidoEvent
    {
        public PedidoEventRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

    }
}
