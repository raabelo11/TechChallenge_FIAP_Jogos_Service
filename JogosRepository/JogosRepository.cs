using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Service.Infrastructure
{
    public class JogosRepository : IJogosRepository
    {
        private readonly ApplicationDbContext _context;

        public JogosRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public bool Adicionar(Jogo jogo)
        {
            _context.Add(jogo);
            return _context.SaveChanges() > 0;
        }

        public bool Atualizar(Jogo jogo)
        {
            var game = _context.Find<Jogo>(jogo.Id);
            _context.Entry(game).CurrentValues.SetValues(jogo);
            return _context.SaveChanges() > 0;
        }

        public List<Jogo> Listar()
        {
            return _context.Jogos.ToList();
        }
    }
}
