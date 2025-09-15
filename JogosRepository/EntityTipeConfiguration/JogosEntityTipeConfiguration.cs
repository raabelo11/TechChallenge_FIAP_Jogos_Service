
using Jogos.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jogos.Service.Domain.EntityTipeConfiguration
{
    public class JogosEntityTipeConfiguration : IEntityTypeConfiguration<Jogo>

    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("Jogos");
            builder.HasKey(j => j.Id);
            builder.Property(j => j.Id).HasColumnName("PK_Jogos").ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(j => j.Nome).IsRequired().HasMaxLength(100).HasColumnName("Nome_Jogo");
            builder.Property(j => j.Descricao).IsRequired().HasMaxLength(200).HasColumnName("Descricao_Jogo");
            builder.Property(j => j.Valor).IsRequired().HasColumnName("Valor_Jogo").HasColumnType("decimal");
            builder.Property(j => j.DataLancamento).IsRequired().HasColumnName("Data_Lancamento");
            builder.Property(j => j.Estudio).IsRequired().HasColumnName("Estudio_Jogo");
            builder.Property(j => j.Genero).IsRequired().HasColumnName("Genero_Jogo");
        }
    }
}
