
using Jogos.Service.Application.Interface;
using Jogos.Service.Application.JogosUseCase;
using Jogos.Service.Application.Utils;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Infrastructure.HttpHandlers;
using Jogos.Service.Infrastructure.Repository;
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
            services.AddScoped<IPedidoEvent, PedidoEventRepository>();
            services.AddScoped<IBiblioteca, BibliotecaRepository>();
            services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            services.AddScoped<ICarrinho, CarrinhoUseCase>();
            services.AddScoped<IPagamentoClient, PagamentoClient>();

            services.AddHttpClient<IPagamentoClient, PagamentoClient>(client =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var apiAddressOptions = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(apiAddressOptions.BaseAddress);
            }).AddHttpMessageHandler<BearerTokenHandler>();
            services.AddHttpClient<ElasticClient>(c =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var apiAddressOptions = serviceProvider.GetRequiredService<IOptions<ElasticSettings>>().Value;
                c.BaseAddress = new Uri(apiAddressOptions.Uri);
                c.DefaultRequestHeaders.Add("Authorization", $"ApiKey {apiAddressOptions.ApiKey}");
            });
        }
    }
}
