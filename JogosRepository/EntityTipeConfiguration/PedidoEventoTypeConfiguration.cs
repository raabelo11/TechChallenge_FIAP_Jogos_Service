
using Jogos.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jogos.Service.Infrastructure.EntityTipeConfiguration
{
    public class PedidoEventoTypeConfiguration : IEntityTypeConfiguration<PedidoEvent>
    {
        public void Configure(EntityTypeBuilder<PedidoEvent> builder)
        {
            builder.ToTable("Pedidos_Event_Sourcing");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(p => p.HashPedido)
                .HasColumnName("Hash_Pedido")
                .IsRequired();

            builder.Property(p => p.EstadoPedido)
                .HasColumnName("Estado_Pedido")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.DataEvento)
                .HasColumnName("Data_Mudanca")
                .HasColumnType("datetime");
        }
    }
}
