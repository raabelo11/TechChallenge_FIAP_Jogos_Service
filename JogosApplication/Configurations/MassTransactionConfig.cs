using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Application.Consumer;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Queue;
using Jogos.Service.Infrastructure.Queue.ModelQueue;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pagamentos.Service.Application.Dtos;

namespace Jogos.Service.Application.Configurations
{
    public static class MassTransactionConfig
    {
        public static void AddMassTransactionConfig(this IServiceCollection services)
        {
            // Configuração do MassTransit apenas para publicação (sem consumidores)
            services.AddMassTransit(x =>
            {
                x.AddConsumer<RabbitMqConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(RabbitMqOptions.Host, h =>
                    {
                        h.Username(RabbitMqOptions.Username);
                        h.Password(RabbitMqOptions.Password);
                    });
                    
                    // Configura explicitamente o nome da exchange/fila para PedidoJogoQueue
                    // Isso garante que o [EntityName("pedido-jogo")] seja respeitado
                    cfg.Message<PedidoJogoQueue>(m =>
                    {
                        m.SetEntityName("pedido-jogo");
                    });

                    cfg.Message<BibliotecaQueue>(m =>
                    {
                        m.SetEntityName("biblioteca-fila");
                    });

                    cfg.ReceiveEndpoint("biblioteca-fila", e =>
                    {
                        e.ConfigureConsumer<RabbitMqConsumer>(context);
                    });
                    // Não é necessário ConfigureEndpoints pois não há consumidores
                    // A configuração do nome da exchange/fila será feita via atributo [EntityName]
                });
            });
            
            // Adiciona o hosted service necessário para o MassTransit funcionar
            services.AddMassTransitHostedService();
        }
    }
}
