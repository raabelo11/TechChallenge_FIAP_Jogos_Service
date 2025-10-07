# Jogos Service - Tech Challenge FIAP

## Descrição
Microsserviço responsável pelo gerenciamento de jogos:
- Listagem de jogos
- Compra de jogos
- Recomendação baseada no histórico do usuário
- Integração com Elasticsearch para busca avançada e métricas

## Funcionalidades
- CRUD de jogos
- Busca e filtros avançados
- Agregações para métricas (mais populares, recentes)
- Event sourcing para rastrear alterações

## Tecnologias
- .NET 8
- Entity Framework Core
- SQL Server
- Elasticsearch
- Serilog
- Docker (opcional)

## Estrutura
- `Jogos.Service.Domain` → Entidades e regras de negócio
- `Jogos.Service.Infrastructure` → Repositórios, contextos e integração com Elasticsearch
- `Jogos.Service.Application` → Casos de uso
- `Jogos.ApiService` → API REST

## Configuração
Configurações no `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=JogosDB;Trusted_Connection=True;"
  },
  "Elasticsearch": {
    "Url": "http://localhost:9200",
    "IndexName": "jogos"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
