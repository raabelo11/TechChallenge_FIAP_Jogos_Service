using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Domain.Interface
{
    public interface IRepositoryGeneric<TEntity> where TEntity : class
    {
        List<TEntity> Listar();
        bool Adicionar(TEntity jogo);
        bool Atualizar(TEntity jogo);

    }
}
