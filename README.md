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

## Mensageria
# Documentação de Mensageria - Sistema de Jogos e Pagamentos

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura de Mensageria](#arquitetura-de-mensageria)
3. [Filas e Exchanges](#filas-e-exchanges)
4. [Fluxo de Comunicação](#fluxo-de-comunicação)
5. [Modelos de Mensagem](#modelos-de-mensagem)
6. [Configuração](#configuração)
7. [Tecnologias Utilizadas](#tecnologias-utilizadas)

---

## 🎯 Visão Geral

O sistema utiliza **RabbitMQ** como broker de mensageria para comunicação assíncrona entre os microsserviços de **Jogos** e **Pagamentos**. A comunicação é implementada através do framework **MassTransit**, que abstrai a complexidade do RabbitMQ e fornece recursos avançados de mensageria.

### Objetivos da Mensageria

- **Desacoplamento**: Os serviços não precisam conhecer diretamente uns aos outros
- **Assíncronia**: Operações que não precisam de resposta imediata são processadas de forma assíncrona
- **Confiabilidade**: Garantia de entrega de mensagens mesmo em caso de falhas temporárias
- **Escalabilidade**: Possibilidade de processar múltiplas mensagens em paralelo

---

## 📬 Filas e Exchanges

O sistema utiliza duas filas principais para comunicação entre os serviços:

### 1. Fila: `pedido-jogo`

**Propósito**: Comunicação do serviço de Jogos para o serviço de Pagamentos

- **Publisher**: Serviço de Jogos (`CarrinhoUseCase`)
- **Consumer**: Serviço de Pagamentos (`RabbitMqConsumer`)
- **Mensagem**: `PedidoJogoQueue`
- **Quando é publicada**: Quando um cliente adiciona um jogo ao carrinho e cria um pedido

### 2. Fila: `biblioteca-fila`

**Propósito**: Comunicação do serviço de Pagamentos para o serviço de Jogos

- **Publisher**: Serviço de Pagamentos (`PagamentoUseCase`)
- **Consumer**: Serviço de Jogos (`RabbitMqConsumer`)
- **Mensagem**: `BibliotecaQueue`
- **Quando é publicada**: Quando um pedido é aprovado ou cancelado no serviço de Pagamentos

---

## 🔄 Fluxo de Comunicação

### Fluxo 1: Criação de Pedido (Jogos → Pagamentos)

```
1. Cliente solicita criação de pedido
   ↓
2. CarrinhoUseCase.Processar() é chamado
   ↓
3. Pedido é salvo no banco de dados do serviço de Jogos
   ↓
4. Mensagem PedidoJogoQueue é publicada na fila "pedido-jogo"
   ↓
5. Serviço de Pagamentos consome a mensagem
   ↓
6. ProcessamentoUseCase.ProcessarPedido() processa o pedido
   ↓
7. Pedido é salvo no banco de dados do serviço de Pagamentos com status "Pendente"
```

**Código relevante:**
- **Publisher**: `JogosApplication/JogosUseCase/CarrinhoUseCase.cs` (linha 66)
- **Consumer**: `Pagamentos.Services.Application/Consumer/RabbitMqConsumer.cs`

### Fluxo 2: Atualização de Status (Pagamentos → Jogos)

```
1. Pedido é aprovado/cancelado no serviço de Pagamentos
   ↓
2. PagamentoUseCase.AtualizarPedido() é chamado
   ↓
3. Status do pedido é atualizado no banco de dados do serviço de Pagamentos
   ↓
4. Mensagem BibliotecaQueue é publicada na fila "biblioteca-fila"
   ↓
5. Serviço de Jogos consome a mensagem
   ↓
6. BibliotecaUseCase.SalvarJogoBiblioteca() processa a mensagem
   ↓
7. Se status = Aprovado (2):
   - Status do pedido é atualizado para "Aprovado"
   - Jogo é adicionado à biblioteca do cliente
8. Se status = Cancelado (3):
   - Status do pedido é atualizado para "Cancelado"
```

**Código relevante:**
- **Publisher**: `Pagamentos.Services.Application/UseCase/PagamentoUseCase.cs` (linha 49)
- **Consumer**: `JogosApplication/Consumer/RabbitMqConsumer.cs`

---

## 📦 Modelos de Mensagem

### PedidoJogoQueue

**Namespace**: `Jogos.Service.Infrastructure.Queue.ModelQueue`

**Fila**: `pedido-jogo`

**Estrutura**:
```csharp
[EntityName("pedido-jogo")]
public class PedidoJogoQueue
{
    public Guid HashPedido { get; set; }      // Identificador único do pedido
    public int IdJogo { get; set; }            // ID do jogo
    public int IdCliente { get; set; }         // ID do cliente
    public int Status { get; set; }            // Status: 1=Pendente, 2=Aprovado, 3=Cancelado
}
```

**Quando é usada**: 
- Publicada pelo serviço de Jogos quando um pedido é criado
- Consumida pelo serviço de Pagamentos para criar o registro de pedido

### BibliotecaQueue

**Namespace**: `Pagamentos.Service.Application.Dtos` (no serviço de Pagamentos)
**Namespace**: `Jogos.Service.Application.Consumer.ModelConsumer` (no serviço de Jogos)

**Fila**: `biblioteca-fila`

**Estrutura**:
```csharp
[EntityName("biblioteca-fila")]
public class BibliotecaQueue
{
    public Guid HashPedido { get; set; }      // Identificador único do pedido
    public int status { get; set; }            // Status: 2=Aprovado, 3=Cancelado
}
```

**Quando é usada**:
- Publicada pelo serviço de Pagamentos quando um pedido é aprovado ou cancelado
- Consumida pelo serviço de Jogos para atualizar o status e adicionar à biblioteca

---

## ⚙️ Configuração

### 1. Configuração do RabbitMQ

As configurações do RabbitMQ estão nos arquivos `appsettings.json` de cada serviço:

**Serviço de Jogos** (`JogosAPI/appsettings.json`):
```json
{
  "RabbitMq": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

**Serviço de Pagamentos** (`Pagamento.ApiService/appsettings.json`):
```json
{
  "RabbitMq": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

## Mensageria
## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura de Mensageria](#arquitetura-de-mensageria)
3. [Filas e Exchanges](#filas-e-exchanges)
4. [Fluxo de Comunicação](#fluxo-de-comunicação)
5. [Modelos de Mensagem](#modelos-de-mensagem)
6. [Configuração](#configuração)
7. [Tecnologias Utilizadas](#tecnologias-utilizadas)

---

## 🎯 Visão Geral

O sistema utiliza **RabbitMQ** como broker de mensageria para comunicação assíncrona entre os microsserviços de **Jogos** e **Pagamentos**. A comunicação é implementada através do framework **MassTransit**, que abstrai a complexidade do RabbitMQ e fornece recursos avançados de mensageria.

### Objetivos da Mensageria

- **Desacoplamento**: Os serviços não precisam conhecer diretamente uns aos outros
- **Assíncronia**: Operações que não precisam de resposta imediata são processadas de forma assíncrona
- **Confiabilidade**: Garantia de entrega de mensagens mesmo em caso de falhas temporárias
- **Escalabilidade**: Possibilidade de processar múltiplas mensagens em paralelo

---

## 🏗️ Arquitetura de Mensageria

```
┌─────────────────┐                    ┌──────────────────┐
│  Serviço Jogos  │                    │ Serviço Pagamentos│
│                 │                    │                   │
│  ┌───────────┐  │                    │  ┌─────────────┐  │
│  │ Publisher │  │                    │  │  Consumer   │  │
│  └─────┬─────┘  │                    │  └──────┬──────┘  │
│        │        │                    │         │          │
│        │ 1. Publica PedidoJogoQueue │         │          │
│        ├─────────────────────────────┼────────>│          │
│        │        │                    │         │          │
│        │        │                    │  ┌──────▼──────┐  │
│        │        │                    │  │ Processa    │  │
│        │        │                    │  │ Pedido      │  │
│        │        │                    │  └──────┬──────┘  │
│        │        │                    │         │          │
│  ┌─────▼─────┐  │                    │  ┌──────▼──────┐  │
│  │ Consumer  │  │                    │  │  Publisher  │  │
│  └─────┬─────┘  │                    │  └──────┬──────┘  │
│        │        │                    │         │          │
│        │        │ 2. Publica BibliotecaQueue   │          │
│        │<─────────────────────────────┼────────┤          │
│        │        │                    │         │          │
│  ┌─────▼─────┐  │                    │         │          │
│  │ Atualiza  │  │                    │         │          │
│  │ Biblioteca│  │                    │         │          │
│  └───────────┘  │                    │         │          │
└─────────────────┘                    └──────────────────┘
         │                                       │
         └───────────────┬───────────────────────┘
                         │
                  ┌──────▼──────┐
                  │   RabbitMQ  │
                  │   Broker    │
                  └─────────────┘
```

---

## 📬 Filas e Exchanges

O sistema utiliza duas filas principais para comunicação entre os serviços:

### 1. Fila: `pedido-jogo`

**Propósito**: Comunicação do serviço de Jogos para o serviço de Pagamentos

- **Publisher**: Serviço de Jogos (`CarrinhoUseCase`)
- **Consumer**: Serviço de Pagamentos (`RabbitMqConsumer`)
- **Mensagem**: `PedidoJogoQueue`
- **Quando é publicada**: Quando um cliente adiciona um jogo ao carrinho e cria um pedido

### 2. Fila: `biblioteca-fila`

**Propósito**: Comunicação do serviço de Pagamentos para o serviço de Jogos

- **Publisher**: Serviço de Pagamentos (`PagamentoUseCase`)
- **Consumer**: Serviço de Jogos (`RabbitMqConsumer`)
- **Mensagem**: `BibliotecaQueue`
- **Quando é publicada**: Quando um pedido é aprovado ou cancelado no serviço de Pagamentos

---

## 🔄 Fluxo de Comunicação

### Fluxo 1: Criação de Pedido (Jogos → Pagamentos)

```
1. Cliente solicita criação de pedido
   ↓
2. CarrinhoUseCase.Processar() é chamado
   ↓
3. Pedido é salvo no banco de dados do serviço de Jogos
   ↓
4. Mensagem PedidoJogoQueue é publicada na fila "pedido-jogo"
   ↓
5. Serviço de Pagamentos consome a mensagem
   ↓
6. ProcessamentoUseCase.ProcessarPedido() processa o pedido
   ↓
7. Pedido é salvo no banco de dados do serviço de Pagamentos com status "Pendente"
```

**Código relevante:**
- **Publisher**: `JogosApplication/JogosUseCase/CarrinhoUseCase.cs` (linha 66)
- **Consumer**: `Pagamentos.Services.Application/Consumer/RabbitMqConsumer.cs`

### Fluxo 2: Atualização de Status (Pagamentos → Jogos)

```
1. Pedido é aprovado/cancelado no serviço de Pagamentos
   ↓
2. PagamentoUseCase.AtualizarPedido() é chamado
   ↓
3. Status do pedido é atualizado no banco de dados do serviço de Pagamentos
   ↓
4. Mensagem BibliotecaQueue é publicada na fila "biblioteca-fila"
   ↓
5. Serviço de Jogos consome a mensagem
   ↓
6. BibliotecaUseCase.SalvarJogoBiblioteca() processa a mensagem
   ↓
7. Se status = Aprovado (2):
   - Status do pedido é atualizado para "Aprovado"
   - Jogo é adicionado à biblioteca do cliente
8. Se status = Cancelado (3):
   - Status do pedido é atualizado para "Cancelado"
```

**Código relevante:**
- **Publisher**: `Pagamentos.Services.Application/UseCase/PagamentoUseCase.cs` (linha 49)
- **Consumer**: `JogosApplication/Consumer/RabbitMqConsumer.cs`

---

## 📦 Modelos de Mensagem

### PedidoJogoQueue

**Namespace**: `Jogos.Service.Infrastructure.Queue.ModelQueue`

**Fila**: `pedido-jogo`

**Estrutura**:
```csharp
[EntityName("pedido-jogo")]
public class PedidoJogoQueue
{
    public Guid HashPedido { get; set; }      // Identificador único do pedido
    public int IdJogo { get; set; }            // ID do jogo
    public int IdCliente { get; set; }         // ID do cliente
    public int Status { get; set; }            // Status: 1=Pendente, 2=Aprovado, 3=Cancelado
}
```

**Quando é usada**: 
- Publicada pelo serviço de Jogos quando um pedido é criado
- Consumida pelo serviço de Pagamentos para criar o registro de pedido

### BibliotecaQueue

**Namespace**: `Pagamentos.Service.Application.Dtos` (no serviço de Pagamentos)
**Namespace**: `Jogos.Service.Application.Consumer.ModelConsumer` (no serviço de Jogos)

**Fila**: `biblioteca-fila`

**Estrutura**:
```csharp
[EntityName("biblioteca-fila")]
public class BibliotecaQueue
{
    public Guid HashPedido { get; set; }      // Identificador único do pedido
    public int status { get; set; }            // Status: 2=Aprovado, 3=Cancelado
}
```

**Quando é usada**:
- Publicada pelo serviço de Pagamentos quando um pedido é aprovado ou cancelado
- Consumida pelo serviço de Jogos para atualizar o status e adicionar à biblioteca

---

## ⚙️ Configuração

### 1. Configuração do RabbitMQ

As configurações do RabbitMQ estão nos arquivos `appsettings.json` de cada serviço:

**Serviço de Jogos** (`JogosAPI/appsettings.json`):
```json
{
  "RabbitMq": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

**Serviço de Pagamentos** (`Pagamento.ApiService/appsettings.json`):
```json
{
  "RabbitMq": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

### 2. Configuração do MassTransit

#### Serviço de Jogos

**Arquivo**: `JogosApplication/Configurations/MassTransactionConfig.cs`

- Configura publicação de `PedidoJogoQueue` na fila `pedido-jogo`
- Configura consumo de `BibliotecaQueue` da fila `biblioteca-fila`
- Consumer registrado: `RabbitMqConsumer` (consome `BibliotecaQueue`)

#### Serviço de Pagamentos

**Arquivo**: `Pagamentos.Services.Application/Configuration/MassTransactionConfig.cs`

- Configura consumo de `PedidoJogoQueue` da fila `pedido-jogo`
- Configura publicação de `BibliotecaQueue` na fila `biblioteca-fila`
- Consumer registrado: `RabbitMqConsumer` (consome `PedidoJogoQueue`)

### 3. Executando o RabbitMQ com Docker Compose

Para iniciar o RabbitMQ, utilize o arquivo `docker-compose.yml`:

```bash
# No diretório do projeto
docker-compose up -d
```

O RabbitMQ estará disponível em:
- **AMQP**: `amqp://localhost:5672`
- **Management UI**: `http://localhost:15672` (usuário: `guest`, senha: `guest`)

### 4. Variáveis de Ambiente

As configurações podem ser ajustadas através do arquivo `.env`:

```env
RABBITMQ_HOST=rabbitmq
RABBITMQ_PORT_AMQP=5672
RABBITMQ_PORT_MANAGEMENT=15672
RABBITMQ_DEFAULT_USER=guest
RABBITMQ_DEFAULT_PASS=guest
```

---

## 🛠️ Tecnologias Utilizadas

### RabbitMQ
- **Versão**: 3-management (imagem Docker)
- **Protocolo**: AMQP 0-9-1
- **Função**: Broker de mensageria
