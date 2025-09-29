using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Infrastructure.Repository
{
    public interface IJogo : IRepositoryGeneric<Jogo>
    {
    }
}
