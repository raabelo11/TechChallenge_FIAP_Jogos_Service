using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Service.Infrastructure.Repository
{
    public class RepositoryGeneric<Tentity>(ApplicationDbContext applicationDbContext) : IRepositoryGeneric<Tentity> where Tentity : class
    {
        private readonly ApplicationDbContext _context = applicationDbContext;


        public bool Adicionar(Tentity tentity)
        {
            _context.Set<Tentity>().Add(tentity);
            return _context.SaveChanges() > 0;
        }

        //public bool Adicionar(Tentity pedidoJogo)
        //{
        //   _context.PedidosJogo.Add(pedidoJogo);
        //    return _context.SaveChanges() > 0;
        //} 

        public bool Atualizar(Tentity tentity)
        {
            //var game = _context.Find<Jogo>(tentity);
            //_context.Entry(game).CurrentValues.SetValues(tentity);
            //return _context.SaveChanges() > 0;
            _context.Set<Tentity>().Update(tentity);
            return _context.SaveChanges() > 0;
        }


        public List<Tentity> Listar()
        {
            return _context.Set<Tentity>().AsNoTracking().ToList();
        }
    }
}
