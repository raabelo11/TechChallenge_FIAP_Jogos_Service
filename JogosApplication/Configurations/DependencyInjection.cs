
using System.Reflection;
using System.Threading.Tasks;
using Jogos.Service.Application.Interface;
using Jogos.Service.Application.JogosUseCase;
using Jogos.Service.Application.Mappings;
using Jogos.Service.Application.Utils;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Jogos.Service.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Jogos.Service.Application.Configurations
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // Adicione aqui os serviços da camada de aplicação
            services.AddScoped<IuseCaseJogos, useCaseJogos>();
            services.AddScoped<IJogo, JogoRepository>();
            services.AddScoped<IPedidoJogo, PedidoJogoRepository>();
            services.AddScoped<IBiblioteca, BibliotecaRepository>();
            services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            services.AddScoped<IClient, ClientAPI>();
            services.AddHttpClient<IClient, ClientAPI>(client =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var apiAddressOptions = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(apiAddressOptions.BaseAddress);
            });

            //services.AddHttpClient<ClientAPI>(client =>
            //{
            //    var serviceProvider = services.BuildServiceProvider();
            //    var apiAddressOptions = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            //    client.BaseAddress = new Uri(apiAddressOptions.BaseAddress);
            //});
        }
    }
}
