using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pagamentos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using MassTransit;

namespace Jogos.Service.Application.Consumer
{
    public class RabbitMqConsumer(IBibliotecaUseCase bibliotecaUseCase) : IConsumer<BibliotecaQueue>
    {
        private readonly IBibliotecaUseCase _bibliotecaUseCase = bibliotecaUseCase;
        public async Task Consume(ConsumeContext<BibliotecaQueue> context)
        {
          await _bibliotecaUseCase.SalvarJogoBiblioteca(context.Message);
        }
    }
}
