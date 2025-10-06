using System;
using System.Collections.Generic;
using System.Linq;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Service.Infrastructure.Repository
{
    public class RepositoryGeneric<Tentity>(ApplicationDbContext applicationDbContext) : IRepositoryGeneric<Tentity> where Tentity : class
    {
        private readonly ApplicationDbContext _context = applicationDbContext;


        public async Task<bool> Adicionar(Tentity tentity)
        {
            _context.Set<Tentity>().Add(tentity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task <bool> Atualizar(Tentity tentity)
        {
            _context.Set<Tentity>().Update(tentity);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task <List<Tentity>> Listar()
        {
            return await _context.Set<Tentity>().AsNoTracking().ToListAsync();
        }
    }
}
