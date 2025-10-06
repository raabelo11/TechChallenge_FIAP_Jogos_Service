using Jogos.Service.Domain.EntityTipeConfiguration;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.EntityTipeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Service.Infrastructure.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<PedidoJogo> PedidosJogo { get; set; }
        public DbSet<Biblioteca> Bibliotecas { get; set; }
        public DbSet<PedidoEvent> PedidosEventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new JogosEntityTipeConfiguration().Configure(modelBuilder.Entity<Jogo>());
            new PedidoJogoEntitytypeConfiguration().Configure(modelBuilder.Entity<PedidoJogo>());
            new BibliotecaEntityTypeConfiguration().Configure(modelBuilder.Entity<Biblioteca>());
            new PedidoEventoTypeConfiguration().Configure(modelBuilder.Entity<PedidoEvent>());
        }

    }
}
