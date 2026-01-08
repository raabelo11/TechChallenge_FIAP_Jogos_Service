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

### Variáveis de Ambiente

O projeto utiliza arquivos `.env` para configuração de variáveis de ambiente, especialmente para o Docker Compose e serviços externos.

#### Configuração Inicial

1. **Copie o arquivo de exemplo**:
   ```bash
   # Windows (PowerShell)
   Copy-Item .env.example .env
   
   # Linux/Mac
   cp .env.example .env
   ```

2. **Edite o arquivo `.env`** com suas configurações locais. O arquivo contém:
   - **RabbitMQ**: Configurações para o broker de mensageria (usado pelo Docker Compose)
   - **Database**: Connection string do banco de dados (opcional, pode ser configurado no appsettings.json)
   - **API Settings**: Endereços base das APIs (opcional)
   - **Elasticsearch**: Credenciais para busca avançada (opcional)

3. **Importante**: 
   - O arquivo `.env` **não deve ser commitado** no repositório (já está no `.gitignore`)
   - Use o `.env.example` como referência para as variáveis disponíveis
   - As variáveis do `.env` são carregadas automaticamente pelo Docker Compose

#### Variáveis Principais

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `RABBITMQ_HOST` | Hostname do RabbitMQ | `rabbitmq` |
| `RABBITMQ_PORT_AMQP` | Porta AMQP | `5672` |
| `RABBITMQ_PORT_MANAGEMENT` | Porta da interface web | `15672` |
| `RABBITMQ_DEFAULT_USER` | Usuário do RabbitMQ | `guest` |
| `RABBITMQ_DEFAULT_PASS` | Senha do RabbitMQ | `guest` |
| `RABBITMQ_CONTAINER_NAME` | Nome do container Docker | `rabbitmq-jogos` |

### Configurações no appsettings.json

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
