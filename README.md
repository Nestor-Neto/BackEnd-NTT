# BackEnd_NTT

## 📋 Descrição
Este projeto serve como uma avaliação para candidatos a desenvolvedor sênior. 
Ele foi criado para medir uma variedade de habilidades e competências necessárias para o papel de desenvolvedor.

Como trabalhamos com `DDD`, para referenciar entidades de outros domínios, usamos o padrão `Identidades Externas` 
com a desnormalização de descrições de entidades.

Portanto, você criará uma API (CRUD completo) que lide com registros de vendas. A API precisa informar:

- Número da venda  
- Data da venda  
- Cliente  
- Valor total da venda  
- Filial onde a venda foi feita  
- Produtos  
- Quantidades  
- Preços unitários  
- Descontos  
- Valor total de cada item  
- Cancelada/Não cancelada
---
## Regras de Negócio
- Compras acima de 4 itens idênticos têm 10% de desconto.  
- Compras entre 10 e 20 itens idênticos têm 20% de desconto.  
- Não é possível vender acima de 20 itens idênticos.  
- Compras abaixo de 4 itens não podem ter desconto.  

Essas regras de negócio definem os níveis de desconto baseados em quantidade e limitações:

## Níveis de Desconto:
- **4+ itens:** desconto de 10%.  
- **10-20 itens:** desconto de 20%.  

## Restrições:
- **Limite máximo:** 20 itens por produto.  
- **Sem descontos permitidos para quantidades abaixo de 4 itens.**
---

## 🚀 Tecnologias e Frameworks

### 💻 Tecnologias Principais
- .NET 8.0 como framework base
- PostgreSQL e MongoDB como bancos de dados
- Docker para containerização
- Mensageria (MessageBrokers)

### 📚 Bibliotecas Principais
- **EntityFrameworkCore (8.0.10)**: Framework principal

#### Persistência e Banco de Dados
- **MongoDB.Driver (2.24.0)**: Para integração com MongoDB
- **MongoDB.Bson (2.24.0)**: Para manipulação de documentos BSON
- **Npgsql.EntityFrameworkCore.PostgreSQL (8.0.8)**: Para integração com PostgreSQL

### 📚 Bibliotecas de Teste

#### Framework de Teste Principal
- **xunit (2.9.2)**
  - Framework de teste mais popular para .NET
  - Usado para: Testes unitários, de integração e funcionais
  - Fornece atributos e asserções para escrita de testes

#### Ferramentas de Execução e Cobertura
- **Microsoft.NET.Test.Sdk (17.11.1)**
  - SDK para execução de testes .NET
  - Usado para: Configuração e execução de testes

- **xunit.runner.visualstudio (2.8.2)**
  - Runner do xUnit para Visual Studio
  - Usado para: Execução de testes no ambiente Visual Studio

- **coverlet.collector (6.0.2)**
  - Ferramenta de cobertura de código
  - Usado para: Medir a cobertura de código dos testes

#### Bibliotecas de Mock e Dados
- **NSubstitute (5.1.0)**
  - Framework de mock para .NET
  - Usado para: Criar mocks e stubs em testes unitários
  - Permite simular comportamentos de dependências

- **Bogus (35.6.1)**
  - Gerador de dados falsos
  - Usado para: Criar dados de teste realistas
  - Ajuda na geração de objetos de teste com dados realistas


### 🏗️ Arquitetura do Projeto

#### 1. Padrão Arquitetural
- **Domain-Driven Design (DDD)**
  - Foco no domínio de negócio
  - Separação clara de responsabilidades
  - Uso de padrão "Identidades Externas" para referência entre domínios

também conhecida como **Onion Architecture** ou **Hexagonal Architecture** em algumas variações.

  #### **Separação em Camadas (Layers)**
  **tipo de Arquitetura**
  - Arquitetura Limpa (Clean Architecture) ou também como **Hexagonal Architecture** em algumas variações.
  
- **Adapters (Adaptadores)**
  - **Driven (Infraestrutura e MessageBroker):** Implementações de persistência (ORM), integrações externas e mensageria.
  - **Drivers (WebApi):** Interface de entrada, responsável por expor a API REST.
- **Core**
  - **Application:** Contém os casos de uso, regras de aplicação e orquestração de serviços.
  - **Domain:** Núcleo do domínio, com entidades, agregados, interfaces e regras de negócio puras.
- **Crosscutting**
  - **Common e IoC:** Funcionalidades transversais, como injeção de dependência, utilitários e configurações comuns.
- **Tests**
  - **Functional, Integration, Unit:** Estrutura clara para diferentes tipos de testes, reforçando a testabilidade da solução.


#### 2. Camadas da Aplicação

##### 📦 Domain Layer (`Ambev.DeveloperEvaluation.Domain`)
- **Responsabilidades:**
  - Entidades de domínio
  - Regras de negócio
  - Interfaces dos repositórios
  - Value Objects
  - Agregados
  - Eventos de domínio

##### 🎯 Application Layer (`Ambev.DeveloperEvaluation.Application`)
- **Responsabilidades:**
  - Casos de uso
  - Orquestração de operações
  - DTOs
  - Validações
  - Mapeamentos
  - Implementação do padrão CQRS (Command Query Responsibility Segregation)

##### 🔧 Infrastructure Layer
- **ORM (`Ambev.DeveloperEvaluation.ORM`)**
  - Implementação do Entity Framework Core
  - Configurações de mapeamento
  - Migrations
  - Contexto do banco de dados

- **MessageBrokers (`Ambev.DeveloperEvaluation.MessagesBrokers`)**
  - Implementação de mensageria
  - Comunicação assíncrona
  - Eventos do sistema

- **IoC (`Ambev.DeveloperEvaluation.IoC`)**
  - Injeção de dependências
  - Configuração de serviços
  - Registro de dependências

##### �� Presentation Layer (`Ambev.DeveloperEvaluation.WebApi`)
- **Responsabilidades:**
  - Controllers
  - Endpoints REST
  - Middlewares
  - Configuração da API
  - Documentação (Swagger)

##### 🔄 Common (`Ambev.DeveloperEvaluation.Common`)
- **Responsabilidades:**
  - Utilitários compartilhados
  - Extensões
  - Constantes
  - Helpers
  - Configurações comuns

#### 3. Padrões de Design Utilizados

1. **CQRS (Command Query Responsibility Segregation)**
   - Separação de operações de leitura e escrita
   - Otimização de performance

2. **Repository Pattern**
   - Abstração do acesso a dados
   - Isolamento da lógica de persistência

3. **Unit of Work**
   - Gerenciamento de transações
   - Consistência de dados

4. **Mediator Pattern**
   - Desacoplamento entre componentes
   - Comunicação via mensagens

5. **Dependency Injection**
   - Inversão de controle
   - Baixo acoplamento

#### 4. Persistência de Dados
- **Banco Relacional (PostgreSQL)**
  - Dados transacionais
  - Relacionamentos complexos

- **Banco NoSQL (MongoDB)**
  - Dados desnormalizados
  - Alta performance em leituras

#### 5. Testes
- **Testes Unitários**
- **Testes de Integração**
- **Testes Funcionais**

#### 6. Aspectos Técnicos
- **Containerização (Docker)**
- **Logging (Serilog)**
- **Autenticação (JWT)**
- **Validação (FluentValidation)**
- **Mapeamento (AutoMapper)**

### 📝 Sistema de Logging

#### Framework Principal
- **Serilog (8.0.3)**
  - Framework de logging estruturado
  - Suporte a múltiplos destinos (sinks)
  - Formatação flexível de logs


## 🗄️ Banco de Dados

#### 1. PostgreSQL (Banco Relacional)
- **Versão**: 8.0.8 (Npgsql.EntityFrameworkCore.PostgreSQL)
- **Configuração**:
  ```json
  "DefaultConnection": "Host=localhost;Database=DeveloperEvaluation;Username=postgres;Password=123456"
  ```
- **Entidades Principais**:
  - `Users`: Gerenciamento de usuários
  - `Sales`: Registro de vendas
  - `SaleItems`: Itens de venda
  - `Customers`: Clientes
  - `Branches`: Filiais

#### 2. MongoDB (Banco NoSQL)
- **Versão**: 2.24.0 (MongoDB.Driver)
- **Configuração**:
  ```json
  "MongoDBConnection":"mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.5.0"
  ```
- **Coleções**:
  - `Sales`: Armazenamento de vendas em formato BSON

## 🔄 Controle de Versão

### Git Flow
- `main`: Código em produção
- `develop`: Desenvolvimento


## ✨ Boas Práticas

### Clean Code
- Nomes descritivos
- Funções pequenas e focadas
- DRY (Don't Repeat Yourself)
- Comentários significativos

####  Padrões de Design
1. **CQRS (Command Query Responsibility Segregation)**
   - Separação de operações de leitura e escrita
   - Otimização de performance
   - Exemplo: Commands e Queries separados

2. **Repository Pattern**
   - Abstração do acesso a dados
   - Isolamento da lógica de persistência
   - Exemplo: Implementações para PostgreSQL e MongoDB

3. **Unit of Work**
   - Gerenciamento de transações
   - Consistência de dados
   - Exemplo: Transações no contexto do EF Core

4. **Mediator Pattern**
   - Desacoplamento entre componentes
   - Comunicação via mensagens
   - Exemplo: Uso do MediatR


### SOLID

#### S: Single Responsibility Principle (Princípio da Responsabilidade Única)
- `MongoDBService`: Responsável apenas por operações no MongoDB
- `DefaultContext`: Responsável apenas pelo contexto do Entity Framework
- `LoggingExtension`: Responsável apenas pela configuração de logs
- `SaleValidator`: Responsável apenas pela validação de vendas

#### O: Open/Closed Principle (Princípio Aberto/Fechado)
- `ISaleRepository`: Interface que permite extensão sem modificação
- `IMongoDBService`: Interface que permite novas implementações
- `IUserService`: Interface que permite diferentes implementações de autenticação
- `IBranchRepository`: Interface que permite diferentes estratégias de persistência

#### L: Liskov Substitution Principle (Princípio da Substituição de Liskov)
- `BaseRepository<T>`: Classe base que pode ser substituída por implementações específicas
- `BaseEntity`: Entidade base que pode ser substituída por entidades específicas
- `BaseService`: Serviço base que pode ser substituído por serviços específicos
- `BaseValidator`: Validador base que pode ser substituído por validadores específicos

#### I: Interface Segregation Principle (Princípio da Segregação de Interface)
- `IReadRepository<T>`: Interface apenas para operações de leitura
- `IWriteRepository<T>`: Interface apenas para operações de escrita
- `IAuthService`: Interface específica para autenticação
- `INotificationService`: Interface específica para notificações

#### D: Dependency Inversion Principle (Princípio da Inversão de Dependência)
- `SaleService`: Depende de `ISaleRepository` e não da implementação concreta
- `UserController`: Depende de `IUserService` e não da implementação concreta
- `BranchService`: Depende de `IBranchRepository` e não da implementação concreta
- `AuthController`: Depende de `IAuthService` e não da implementação concreta


## 🧪 Testes

#### Framework e Ferramentas
- **xUnit (2.9.2)**: Framework principal de testes
- **NSubstitute (5.1.0)**: Framework de mock
- **FluentAssertions (6.12.0)**: Biblioteca de asserções
- **Bogus (35.6.1)**: Gerador de dados falsos
- **Coverlet (6.0.2)**: Ferramenta de cobertura de código
####  Padrões de Teste

1. **Arrange-Act-Assert (AAA)**
   - Arrange: Preparação dos dados
   - Act: Execução da ação
   - Assert: Verificação dos resultados

2. **Given-When-Then**
   - Given: Contexto inicial
   - When: Ação executada
   - Then: Resultado esperado

### Testes Unitários
- **xUnit**: Framework de testes
- **FluentAssertions**: Para assertions mais legíveis e expressivas
- **Bogus**: Para geração de dados fake realistas
- **NSubstitute**: Para mocking de dependências


### Testes de Integração

#### 1. Framework e Ferramentas
- **xUnit (2.9.2)**: Framework principal
- **Entity Framework Core (8.0.10)**: Para testes com banco de dados
- **PostgreSQL (8.0.8)**: Banco de dados para testes
- **MongoDB (2.24.0)**: Banco NoSQL para testes
- **NSubstitute (5.1.0)**: Para mocks quando necessário
- **Bogus (35.6.1)**: Geração de dados de teste

#### 2. Áreas de Teste

1. **Testes de Repositório**
   - Operações CRUD
   - Queries complexas
   - Transações
   - Relacionamentos

2. **Testes de Serviço**
   - Lógica de negócio
   - Integração entre camadas
   - Validações
   - Regras de negócio

3. **Testes de Banco de Dados**
   - PostgreSQL
   - MongoDB
   - Migrations
   - Schemas

## 🌐 APIs REST

### 🔐 Autenticação (`AuthController`)
- **POST** `/api/Auth`
  - Autenticação de usuários
  - Retorna token JWT
  - Status: 200 (OK), 400 (Bad Request), 401 (Unauthorized), 500 (Internal Server Error)

### 👥 Usuários (`UsersController`)
- **POST** `/api/Users`
  - Criação de usuários
  - Status: 201 (Created), 400, 409 (Conflict), 500
- **GET** `/api/Users/{id}`
  - Busca usuário por ID
  - Status: 200, 400, 404 (Not Found), 500
- **DELETE** `/api/Users/{id}`
  - Remove usuário
  - Status: 200, 400, 404, 500

### 🛍️ Vendas (`SalesController`)
- **POST** `/api/Sales`
  - Criação de vendas
  - Status: 201, 400, 409, 500
- **GET** `/api/Sales`
  - Lista todas as vendas (com paginação)
  - Parâmetros: page, size
  - Status: 201, 400, 409, 500
- **GET** `/api/Sales/{id}`
  - Busca venda por ID
  - Status: 201, 400, 409, 500
- **PUT** `/api/Sales/{id}/cancel`
  - Cancela uma venda
  - Status: 201, 400, 409, 500

### 👤 Clientes (`CustomerController`)
- **POST** `/api/Customer`
  - Criação de clientes
  - Status: 201, 400, 409, 500
- **GET** `/api/Customer`
  - Lista todos os clientes (com paginação)
  - Parâmetros: page, size
  - Status: 201, 400, 409, 500
- **GET** `/api/Customer/{id}`
  - Busca cliente por ID
  - Status: 201, 400, 409, 500

### 📝 Características da API

1. **Padrões REST**
   - Uso correto dos verbos HTTP
   - URLs semânticas
   - Respostas padronizadas

2. **Respostas Padronizadas**
   - Todas as respostas seguem o formato `ApiResponse` ou `ApiResponseWithData<T>`
   - Incluem:
     - Success (boolean)
     - Message (string)
     - Data (quando aplicável)

3. **Segurança**
   - Autenticação via JWT
   - Validação de requisições
   - Tratamento de exceções

4. **Documentação**
   - Swagger/OpenAPI implementado
   - Comentários XML nas controllers
   - Descrição de status codes

5. **Validação**
   - Validação de modelos
   - Tratamento de erros de negócio
   - Respostas de erro padronizadas

6. **Paginação**
   - Suporte a paginação em listagens
   - Parâmetros: page e size

## 🛠️ Como Executar

### 🖥️ Pré-requisitos necessários para executar o sistema

1. **Sistema Operacional**
   - Windows 10/11 ou Linux
   - Docker Desktop instalado e configurado

2. **Ferramentas de Desenvolvimento**
   - .NET 8.0 SDK
   - Visual Studio 2022 ou VS Code
   - Git

### 🐳 Containers Necessários

1. **API (.NET 8.0)**
   - Porta: 8080 (HTTP) e 8081 (HTTPS)
   - Ambiente: Development

2. **PostgreSQL (13)**
   - Porta: 5432
   - Credenciais:
     - Database: developer_evaluation
     - Usuário: developer
     - Senha: ev@luAt10n

3. **MongoDB (Latest)**
   - Porta: 27017
   - Credenciais:
     - Database: DeveloperEvaluation
     - Usuário: developer
     - Senha: ev@luAt10n

4. **Redis (7.4.1)**
   - Porta: 6379
   - Senha: ev@luAt10n


### 📥 Instalação

#### 1. Clone do Repositório
```bash
# Clone o repositório
git clone https://github.com/Nestor-Neto/BackEnd-NTT.git

# Entre no diretório do projeto
cd BackEnd-NTT
```
#### 2. Configuração do Ambiente
```bash
# Restaure os pacotes NuGet
dotnet restore

# Configure as variáveis de ambiente
# Crie um arquivo .env na raiz do projeto com:
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_HTTP_PORTS=8080
ASPNETCORE_HTTPS_PORTS=8081
```

#### 3. Configuração dos Bancos de Dados
```bash
# Execute as migrations do Entity Framework
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM
```

#### 4. Executando com Docker
```bash
# Build e execução dos containers
docker-compose up --build

# Para executar em background
docker-compose up -d
```

#### 5. Executando Localmente
```bash
# Execute a API
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

### 🔍 Verificação da Instalação

1. **API**
   - Acesse: `https://localhost:5119/swagger`
   - Verifique se a interface Swagger está disponível

2. **Bancos de Dados**
   - PostgreSQL: `localhost:5432`
   - MongoDB: `localhost:27017`
   - Redis: `localhost:6379`

### 🧪 Executando os Testes

#### 1. Testes Unitários
```bash
# Executar todos os testes unitários
dotnet test tests/Ambev.DeveloperEvaluation.Unit

# Executar testes específicos
dotnet test tests/Ambev.DeveloperEvaluation.Unit --filter "FullyQualifiedName~UserTests"
```
#### 2. Testes Integração
```bash
# Executar todos os testes de integração
dotnet test tests/Ambev.DeveloperEvaluation.Integration

# Executar testes específicos
dotnet test tests/Ambev.DeveloperEvaluation.Integration --filter "FullyQualifiedName~CustomerTests"
```
#### 3. Testes Funcionais
```bash
# Executar todos os testes funcionais
dotnet test tests/Ambev.DeveloperEvaluation.Functional

# Executar testes específicos
dotnet test tests/Ambev.DeveloperEvaluation.Functional --filter "FullyQualifiedName~SalesTests"
