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
            builder.Property(b => b.jogos).HasColumnName("Id_Jogos");
            builder.Property(b => b.IdCliente).HasColumnName("Id_Cliente");
            builder.HasMany(b => b.jogos).WithMany(j => j.Bibliotecas).UsingEntity(jt => jt.ToTable("Jogos"));
        }
    }
}
