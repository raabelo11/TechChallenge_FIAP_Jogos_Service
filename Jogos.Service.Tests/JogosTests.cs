using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Application.JogosUseCase;
using Jogos.Service.Application.Utils;
using Jogos.Service.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Jogos.Service.Tests
{
    public class JogosTests
    {
        [Fact(DisplayName = "Não salvar jogo sem o estudio")]
        public async Task NaoSalvarJogoComEstudioGeneroIncorreto()
        {
            // Arrange
            //Aqui você configuraria os serviços necessários, possivelmente usando mocks
            var mockServico = new Mock<IuseCaseJogos>();
            var mockLog = new Mock<ILogger<useCaseJogos>>();
            var mockMapper = new Mock<AutoMapper.IMapper>();
            var mockRepo = new Mock<IJogo>();
            ElasticClient elasticClient = new ElasticClient(new HttpClient());

            var useCaseJogos = new useCaseJogos(mockRepo.Object,mockMapper.Object, mockLog.Object, elasticClient);
            // Act
            var response = await useCaseJogos.Create(new JogoDto
            {
                Nome = "Jogo Teste",
                DataLancamento = DateTime.Now
            });
            // Assert
            Assert.False(response.Ok);
        }
        [Fact(DisplayName = "Salvar jogo com estudio e genero correto")]
        public async Task SalvarJogoComEstudioGeneroCorreto()
        {
            // Arrange
            //Aqui você configuraria os serviços necessários, possivelmente usando mocks
            var mockServico = new Mock<IuseCaseJogos>();
            var mockLog = new Mock<ILogger<useCaseJogos>>();
            var mockMapper = new Mock<AutoMapper.IMapper>();
            var mockRepo = new Mock<IJogo>();
            ElasticClient elasticClient = new ElasticClient(new HttpClient());
            mockRepo.Setup(repo => repo.Adicionar(It.IsAny<Domain.Models.Jogo>())).ReturnsAsync(true);
            var useCaseJogos = new useCaseJogos(mockRepo.Object, mockMapper.Object, mockLog.Object, elasticClient);
            // Act
            var response = await useCaseJogos.Create(new JogoDto
            {
                Nome = "Jogo Teste",
                DataLancamento = DateTime.Now,
                Estudio = Domain.Enums.Estudio.Nintendo,
                Genero = Domain.Enums.Genero.Aventura,
                Valor = 100
            });
            // Assert
            Assert.True(response.Ok);
        }
    }
}
