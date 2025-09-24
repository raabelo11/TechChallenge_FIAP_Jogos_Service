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
    public class BibliotecaRepository(ApplicationDbContext dbContext) : IBiblioteca
    {
        private readonly ApplicationDbContext _context = dbContext;
        public bool Adicionar(Biblioteca biblioteca)
        {
            _context.Bibliotecas.Add(biblioteca);
            return _context.SaveChanges() > 0;
        }

        public List<Biblioteca> Listar(int id)
        {
            return _context.Bibliotecas.ToList().FindAll(x => x.IdCliente == id);
        }
    }
}
