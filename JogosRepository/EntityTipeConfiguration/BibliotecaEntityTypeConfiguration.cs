using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jogos.Service.Infrastructure.EntityTipeConfiguration
{
    public class BibliotecaEntityTypeConfiguration : IEntityTypeConfiguration<Biblioteca>
    {
        public void Configure(EntityTypeBuilder<Biblioteca> builder)
        {
            builder.ToTable("Biblioteca");
            builder.HasKey(e => new { e.IdCliente, e.IdJogo });
            builder.Property(b => b.IdJogo).HasColumnName("Id_Jogo");
            builder.Property(b => b.IdCliente).HasColumnName("Id_Cliente");
            builder.HasOne(b => b.Jogo).WithMany(j => j.Bibliotecas).HasForeignKey(b => b.IdJogo);
        }
    }
}
