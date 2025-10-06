
using Jogos.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jogos.Service.Infrastructure.EntityTipeConfiguration
{
    public class PedidoJogoEntitytypeConfiguration : IEntityTypeConfiguration<PedidoJogo>
    {
        public void Configure(EntityTypeBuilder<PedidoJogo> builder)
        {
            builder.ToTable("Pedidos_Jogos");
            builder.HasKey(p => p.HashPedido);
            builder.Property(p => p.HashPedido).HasColumnName("Hash_Pedido").IsRequired();
            builder.Property(p => p.IdJogo).HasColumnName("Id_Jogo").IsRequired();
            builder.Property(p => p.IdCliente).HasColumnName("Id_Cliente").IsRequired();
            builder.Property(p => p.Status).HasColumnName("Status_Pedido").IsRequired();
        }
    }
}
