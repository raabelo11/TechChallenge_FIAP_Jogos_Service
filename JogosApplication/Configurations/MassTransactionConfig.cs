using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jogos.Service.Infrastructure.Queue;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Jogos.Service.Application.Configurations
{
    public static class MassTransactionConfig
    {
        public static void AddMassTransactionConfig(this IServiceCollection services)
        {
            // Configurações relacionadas a transações em massa podem ser adicionadas aqui
             services.AddMassTransit(x =>
             {
                 x.UsingRabbitMq((context, cfg) =>
                 {
                     cfg.Host(RabbitMqOptions.Host, h =>
                     {
                         h.Username(RabbitMqOptions.Username);
                         h.Password(RabbitMqOptions.Password);
                     });
                     // Configuração de consumidores, sagas, etc., pode ser adicionada aqui
                 });
             });
        }
    }
}
