
using System.Threading.Tasks;
using Jogos.Service.Application.Interface;
using Jogos.Service.Application.JogosUseCase;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Jogos.Service.Application.Configurations
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // Adicione aqui os serviços da camada de aplicação
            
            services.AddScoped<IuseCaseJogos, useCaseJogos>();
            services.AddScoped<IJogos, JogosRepository>();

        }
    }
}
