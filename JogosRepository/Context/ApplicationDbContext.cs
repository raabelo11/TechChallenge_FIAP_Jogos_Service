using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.EntityTipeConfiguration;
using Jogos.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Service.Infrastructure.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }
        public DbSet<Jogo> Jogos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new JogosEntityTipeConfiguration().Configure(modelBuilder.Entity<Jogo>());
        }

    }
}
