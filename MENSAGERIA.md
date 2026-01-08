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

### MassTransit
- **Versão**: 8.5.7
- **Função**: Framework de mensageria que abstrai o RabbitMQ
- **Recursos utilizados**:
  - Publishers (publicação de mensagens)
  - Consumers (consumo de mensagens)
  - EntityName (nomenclatura de filas)
  - ReceiveEndpoint (configuração de endpoints)

### RabbitMQ.Client
- **Versão**: 7.2.0
- **Função**: Cliente .NET para RabbitMQ (usado internamente pelo MassTransit)

---

## 🔍 Monitoramento e Debugging

### Interface de Gerenciamento do RabbitMQ

Acesse `http://localhost:15672` para:
- Visualizar filas e mensagens
- Monitorar conexões e canais
- Ver estatísticas de mensagens
- Gerenciar exchanges e bindings

### Logs

Ambos os serviços registram logs importantes:
- Publicação de mensagens
- Recebimento de mensagens
- Processamento de mensagens
- Erros durante o processamento

**Exemplo de logs**:
```
[INFO] Mensagem publicada na fila RabbitMQ para o pedido {HashPedido}
[INFO] Mensagem recebida para processamento de pedido. HashPedido: {HashPedido}
[INFO] Pedido processado e salvo com sucesso. HashPedido: {HashPedido}
[ERROR] Erro ao processar mensagem do RabbitMQ. HashPedido: {HashPedido}
```

---

## 📝 Observações Importantes

1. **Idempotência**: As operações devem ser idempotentes, pois uma mensagem pode ser processada múltiplas vezes em caso de retry.

2. **Tratamento de Erros**: Erros durante o processamento são logados e a exceção é relançada para que o MassTransit possa fazer retry se configurado.

3. **Status do Pedido**: 
   - `1` = Pendente
   - `2` = Aprovado
   - `3` = Cancelado

4. **HashPedido**: Identificador único (GUID) usado para correlacionar mensagens entre os serviços.

5. **Sincronização**: O serviço de Jogos mantém seu próprio estado de pedidos, e o serviço de Pagamentos também mantém seu estado. A mensageria sincroniza essas informações.

---

## 🔄 Retry Policy (Política de Retry)

O sistema implementa políticas de retry para garantir que mensagens com falha temporária sejam reprocessadas automaticamente.

### Configuração Atual

Ambos os serviços estão configurados com duas estratégias de retry combinadas:

1. **Retry Imediato**: 3 tentativas imediatas para erros transitórios rápidos
2. **Retry Exponencial**: 5 tentativas com intervalos crescentes (1s, 5s, 10s, 30s, 60s)

### Como Funciona

Quando uma mensagem falha durante o processamento:

1. **Primeira tentativa**: Processamento normal
2. **Retries imediatos**: 3 tentativas sem espera (útil para erros de conexão rápida)
3. **Retries exponenciais**: 5 tentativas com intervalos crescentes:
   - 1ª tentativa: após 1 segundo
   - 2ª tentativa: após 5 segundos
   - 3ª tentativa: após 10 segundos
   - 4ª tentativa: após 30 segundos
   - 5ª tentativa: após 60 segundos

**Total**: Até 9 tentativas (1 inicial + 3 imediatas + 5 exponenciais)

### Tipos de Retry Disponíveis

#### 1. Retry Imediato
```csharp
e.UseMessageRetry(r => r.Immediate(3));
```
- Útil para: Erros transitórios que podem ser resolvidos rapidamente
- Exemplo: Timeout de conexão, deadlock temporário

#### 2. Retry com Intervalo Fixo
```csharp
e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
```
- Útil para: Erros que precisam de tempo fixo para se resolver
- Exemplo: Serviço externo temporariamente indisponível

#### 3. Retry Exponencial (Atual)
```csharp
e.UseMessageRetry(r => r.Exponential(
    retryLimit: 5,
    minInterval: TimeSpan.FromSeconds(1),
    maxInterval: TimeSpan.FromSeconds(60),
    intervalDelta: TimeSpan.FromSeconds(5)));
```
- Útil para: Erros que podem levar tempo variável para se resolver
- Exemplo: Sobrecarga de banco de dados, problemas de rede

#### 4. Retry com Filtro de Exceções
```csharp
e.UseMessageRetry(r => r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(5))
    .Handle<SqlException>()
    .Ignore<ArgumentException>());
```
- Útil para: Aplicar retry apenas para exceções específicas
- Ignora exceções que não devem ser retentadas (ex: validação)

### Customizando Retry por Consumer

Você pode configurar retry específico para cada consumer:

```csharp
cfg.ReceiveEndpoint("biblioteca-fila", e =>
{
    // Retry específico para este endpoint
    e.UseMessageRetry(r => r.Exponential(5, 
        TimeSpan.FromSeconds(1), 
        TimeSpan.FromSeconds(60), 
        TimeSpan.FromSeconds(5)));
    
    e.ConfigureConsumer<RabbitMqConsumer>(context);
});
```

### Dead Letter Queue (DLQ)

Após esgotar todas as tentativas de retry, a mensagem pode ser movida para uma Dead Letter Queue. Para configurar:

```csharp
cfg.ReceiveEndpoint("biblioteca-fila", e =>
{
    e.UseMessageRetry(r => r.Exponential(5, 
        TimeSpan.FromSeconds(1), 
        TimeSpan.FromSeconds(60), 
        TimeSpan.FromSeconds(5)));
    
    // Configura DLQ para mensagens que falharam após todos os retries
    e.PublishFaults = true;
    
    e.ConfigureConsumer<RabbitMqConsumer>(context);
});
```

### Monitoramento de Retries

Os retries são automaticamente logados pelo MassTransit. Você verá logs como:

```
[WARN] Retry attempt 1 of 5 for message {MessageId}
[WARN] Retry attempt 2 of 5 for message {MessageId}
[ERROR] Message failed after all retry attempts: {MessageId}
```

### Boas Práticas

1. **Idempotência**: Garanta que suas operações sejam idempotentes, pois serão executadas múltiplas vezes
2. **Logging**: Sempre logue tentativas de retry para facilitar debugging
3. **Timeouts**: Configure timeouts apropriados para evitar retries desnecessários
4. **Dead Letter Queue**: Configure DLQ para mensagens que falharam permanentemente
5. **Métricas**: Monitore a taxa de retry para identificar problemas sistêmicos

---

## 🚀 Próximos Passos

- [x] Implementar retry policy no MassTransit
- [ ] Adicionar dead letter queue para mensagens com falha
- [ ] Implementar circuit breaker para resiliência
- [ ] Adicionar métricas e monitoramento avançado
- [ ] Implementar versionamento de mensagens

---

**Última atualização**: Dezembro 2024
