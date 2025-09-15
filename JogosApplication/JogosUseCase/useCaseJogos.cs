using AutoMapper;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Application.Interface;
using Jogos.Service.Domain.Enums;
using Jogos.Service.Domain.Interface;
using Jogos.Service.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Jogos.Service.Application.JogosUseCase
{
    public class useCaseJogos : IuseCaseJogos
    {
        private readonly IJogosRepository _jogos;
        private readonly IMapper _mapper;
        private readonly ILogger<useCaseJogos> _logger;
        public useCaseJogos(IJogosRepository jogos, IMapper mapper, ILogger<useCaseJogos> logger)
        {
            _jogos = jogos;
            _mapper = mapper;
            _logger = logger;
        }
        public JogosResponse Create(JogoDto request)
        {
            try
            {
                _logger.LogInformation("Criando o jogo");
                if (Enum.IsDefined(typeof(Genero), request.Genero) == false || Enum.IsDefined(typeof(Estudio), request.Estudio) == false)
                {
                    _logger.LogWarning("Não foi possível criar o jogo, verificar o id do Estúdio ou Gênero");
                    return new JogosResponse
                    {
                        Ok = false,
                        Errors = new string[] { "Estúdio ou Genêro inválido" }
                    };
                }
                _logger.LogInformation($"o Jogo {request.Nome} foi criado!");

                return new JogosResponse
                {

                    Ok = _jogos.Adicionar(_mapper.Map<Jogo>(request))
                };
            }
            catch (Exception ex)
            {
                return new JogosResponse
                {
                    Ok = false,
                    Errors = new string[] { ex.Message }
                };

            }
        }

        public JogosResponse listarJogos()
        {
            try
            {
                var jogos = _jogos.Listar();
                return new JogosResponse
                {
                    Ok = true,
                    data = _mapper.Map<List<JogoDto>>(jogos)
                };

            }
            catch (Exception ex)
            {
                return new JogosResponse
                {
                    Ok = false,
                    Errors = new string[] { ex.Message }
                };
            }
        }

        public JogosResponse Update(JogoRequest request)
        {
            _logger.LogInformation("Localizando o jogo");
            try
            {
                var JogoExistente = _jogos.Listar().FirstOrDefault(x => x.Id == request.Id);
                if (JogoExistente == null)
                {
                    _logger.LogWarning("Jogo não encontrado");
                    return new JogosResponse
                    {
                        Ok = false,
                        Errors = new string[] { "Jogo não encontrado" }
                    };
                }
                var jogoMapeado = _mapper.Map<Jogo>(request);
                return new JogosResponse
                {
                    Ok = _jogos.Atualizar(jogoMapeado)
                };
            }
            catch (Exception ex)
            {
                return new JogosResponse
                {
                    Ok = false,
                    Errors = new string[] { ex.Message }
                };

            }
        }

        public JogosResponse listarCategorias()
        {
            return new JogosResponse
            {
                Ok = true,
                data = ListarEnum()
            };
        }
        public JogosResponse listarEstudios()
        {
            return new JogosResponse
            {
                Ok = true,
                data = ListarEnumEstudios()
            };
        }
        private List<string> ListarEnum()
        {
            var categorias = Enum.GetValues(typeof(Genero));
            var index = 0;
            List<string> listaCategorias = new List<string>();
            foreach (var categoria in categorias)
            {
                string IdNome = $"{index} - {categoria.ToString()}";
                listaCategorias.Add(IdNome);
                index++;
            }
            return listaCategorias;
        }
        private List<string> ListarEnumEstudios()
        {
            var categorias = Enum.GetValues(typeof(Estudio));
            var index = 0;
            List<string> listaEstudios = new List<string>();
            foreach (var categoria in categorias)
            {
                string IdNome = $"{index} - {categoria.ToString()}";
                listaEstudios.Add(IdNome);
                index++;
            }
            return listaEstudios;
        }

    }
}
