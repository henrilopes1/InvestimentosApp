# 📈 Sistema de Gestão de Investimentos

Um sistema completo de gestão de investimentos desenvolvido em C# com .NET 8, utilizando Oracle Database e seguindo arquitetura em camadas. **Integrado com APIs externas reais para dados de mercado financeiro.**

## 🌐 **APLICAÇÃO EM PRODUÇÃO NA AZURE CLOUD**

### 🚀 **Acesse a API Online:**
- **📋 Swagger UI:** https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/swagger/index.html
- **☁️ Deploy:** Automático via GitHub Actions
- **🔧 Infraestrutura:** Azure App Service (East US 2)

### 🧪 **Teste os Endpoints Online:**
```bash
# Listar investidores
GET https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/investidores

# Cotação em tempo real (Alpha Vantage)
GET https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/alphavantage/quote/AAPL

# Dados de mercado (MarketStack)
GET https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/marketstack/eod/latest?symbols=MSFT
```

## 🎯 Funcionalidades Principais

### 👥 Gestão de Investidores
- ✅ Criar, listar, atualizar e excluir investidores
- ✅ Validações de CPF, email e campos obrigatórios
- ✅ Perfis de risco (Conservador, Moderado, Arrojado)
- ✅ **23 endpoints avançados com LINQ** para buscas complexas
- ✅ Busca por nome, email, perfil de risco, faixa etária
- ✅ Estatísticas agregadas (média de idade, saldo total, distribuição por perfil)
- ✅ Filtros combinados e ordenação

### 📈 Gestão de Investimentos
- ✅ Criar, listar, atualizar e excluir investimentos
- ✅ Tipos: Renda Fixa, Renda Variável, Fundos Imobiliários
- ✅ Controle de rentabilidade e valores
- ✅ **Endpoints LINQ avançados** para análises detalhadas
- ✅ Filtros por tipo, rentabilidade, período, status
- ✅ Análises estatísticas (média de rentabilidade, total investido)
- ✅ Relacionamento com investidores

### 🌐 APIs Externas Integradas

#### 📊 Alpha Vantage API (Dados Financeiros)
- ✅ **Cotações em tempo real** de ações (AAPL, GOOGL, MSFT, etc.)
- ✅ **Busca de símbolos** por palavras-chave
- ✅ **Dados históricos** diários e intraday
- ✅ **Indicadores técnicos** (SMA, EMA, RSI, etc.)
- ✅ **Dados econômicos** (PIB, inflação, desemprego)
- ✅ **Notícias de mercado** em tempo real
- ✅ **6 endpoints especializados** para análises financeiras

#### 📈 MarketStack API (Mercado de Ações)
- ✅ **End-of-Day Data** - Preços de fechamento diários
- ✅ **Intraday Data** - Dados em tempo real (1min, 5min, 1hour, etc.)
- ✅ **170.000+ tickers** de 50+ países
- ✅ **70+ bolsas de valores** mundiais
- ✅ **Dividendos e splits** históricos
- ✅ **750+ índices de mercado**
- ✅ **7 endpoints funcionais** para análise de mercado (testados e validados)

### 📄 Manipulação de Arquivos
- ✅ Exportação de investidores para JSON
- ✅ Importação de investidores via JSON
- ✅ Exportação de investimentos para JSON
- ✅ Importação de investimentos via JSON
- ✅ Exportação adicional para TXT
- ✅ Validações robustas nos arquivos importados

### 🌐 Interface Web API
- ✅ API RESTful completa com **38+ endpoints funcionais**
- ✅ Documentação interativa com Swagger UI
- ✅ Testes integrados na interface (100% validados)
- ✅ Tratamento de erros padronizado
- ✅ **Rate limiting** e controle de acesso às APIs externas
- ✅ **Logs detalhados** para monitoramento e debugging

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** com três camadas principais:

```
InvestimentosApp/
├── src/
│   ├── InvestimentosApp.Domain/         # 🏛️ Entidades e Contratos
│   │   ├── Models/                      # Investidor.cs, Investimento.cs
│   │   └── Interfaces/                  # Contratos dos repositórios
│   │
│   ├── InvestimentosApp.Data/           # 💾 Acesso a Dados
│   │   ├── Context/                     # Entity Framework
│   │   └── Repositories/                # Implementação dos repositórios
│   │
│   └── InvestimentosApp.API/            # 🌐 API RESTful
│       ├── Controllers/                 # Endpoints da API
│       │   ├── InvestidoresController   # CRUD + 23 endpoints LINQ
│       │   ├── InvestimentosController  # CRUD + endpoints LINQ
│       │   ├── AlphaVantageController   # 6 endpoints financeiros
│       │   ├── MarketStackController    # 10 endpoints de mercado
│       │   └── ArquivosController       # Import/Export
│       ├── Services/                    # Lógica de aplicação
│       │   ├── AlphaVantageService      # Integração Alpha Vantage
│       │   ├── MarketStackService       # Integração MarketStack
│       │   └── ArquivoService           # Processamento de arquivos
│       ├── Models/ExternalAPIs/         # Modelos das APIs externas
│       └── Program.cs                   # Configuração
└── InvestimentosApp.sln
```

**Camadas:**
- **Domain**: Entidades de negócio e interfaces
- **Data**: Repositórios com LINQ avançado e acesso ao Oracle Database
- **API**: Controllers, serviços e integração com APIs externas

## 🛠️ Tecnologias Utilizadas

- **Framework**: .NET 8.0
- **Linguagem**: C# 12
- **Cloud**: Microsoft Azure App Service
- **Deploy**: GitHub Actions (CI/CD automático)
- **Banco de Dados**: Oracle Database (FIAP)
- **ORM**: Entity Framework Core com LINQ avançado
- **Provedor Oracle**: Oracle.EntityFrameworkCore v7.21.12
- **APIs Externas**: Alpha Vantage + MarketStack
- **HTTP Client**: HttpClientFactory com retry policies
- **Serialização**: Newtonsoft.Json
- **Documentação API**: Swagger/OpenAPI (Swashbuckle.AspNetCore v6.5.0)
- **Arquitetura**: Clean Architecture (Camadas)
- **Padrões**: Repository Pattern, Dependency Injection, Service Pattern

## 🚀 Como Acessar/Executar

### 🌐 **Opção 1: Acessar Online (Recomendado)**
A aplicação já está rodando na Azure Cloud:

- **📋 Swagger (Documentação Interativa):** https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/swagger/index.html

**✅ Pronto para usar! Não precisa de configuração local.**

### 💻 **Opção 2: Executar Localmente**

#### **Pré-requisitos**
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code
- Acesso ao banco Oracle SQL Developer
- **Chaves das APIs externas** (gratuitas):
  - [Alpha Vantage](https://www.alphavantage.co/support/#api-key) 
  - [MarketStack](https://marketstack.com/signup/free)

#### **Configuração Local**
1. Clone o repositório
2. **Configure suas credenciais Oracle** no `Program.cs`:
   ```csharp
   builder.Services.AddScoped<AppDbContext>(provider => 
       new AppDbContext("SEU_RM_AQUI", "SUA_SENHA_AQUI"));
   ```
3. **Configure as chaves das APIs externas** no `appsettings.json`:
   ```json
   {
     "AlphaVantage": {
       "ApiKey": "YOUR_ALPHA_VANTAGE_API_KEY_HERE",
       "BaseUrl": "https://www.alphavantage.co/query",
       "RateLimitPerMinute": 5
     },
     "MarketStack": {
       "ApiKey": "YOUR_MARKETSTACK_API_KEY_HERE",
       "BaseUrl": "https://api.marketstack.com/v1"
     }
   }
   ```
4. **Obtenha as chaves das APIs gratuitas:**
   - [Alpha Vantage](https://www.alphavantage.co/support/#api-key) - Gratuita (5 calls/min)
   - [MarketStack](https://marketstack.com/signup/free) - Gratuita (1000 calls/mês)
5. **Crie as tabelas no Oracle** (script SQL completo abaixo)

#### **Execução Local**
```bash
cd src/InvestimentosApp.API
dotnet run
```

#### **Acesso Local**
- **Swagger UI**: http://localhost:5000/swagger/index.html
- **API Base**: http://localhost:5000/api/

---

## 🌐 **URLs de Produção (Azure)**

### **📋 Documentação e Testes:**
- **Swagger UI:** https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/swagger/index.html

### **🧪 Exemplos de Endpoints Online:**
```bash
# Listar investidores
curl https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/investidores

# Cotação da Apple (Alpha Vantage)
curl https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/alphavantage/quote/AAPL

# Dados de mercado da Microsoft (MarketStack)
curl https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/marketstack/eod/latest?symbols=MSFT

# Busca avançada de investidores por perfil
curl https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/api/investidores/perfil/Arrojado
```

## 📊 Endpoints da API

### 👥 Investidores (23 Endpoints LINQ)
**CRUD Básico:**
- `GET /api/Investidores` - Listar todos os investidores
- `GET /api/Investidores/{id}` - Buscar investidor por ID
- `POST /api/Investidores` - Criar novo investidor
- `PUT /api/Investidores/{id}` - Atualizar investidor
- `DELETE /api/Investidores/{id}` - Excluir investidor

**Buscas Avançadas com LINQ:**
- `GET /api/Investidores/search/name/{nome}` - Busca por nome
- `GET /api/Investidores/search/email/{email}` - Busca por email
- `GET /api/Investidores/search/cpf/{cpf}` - Busca por CPF
- `GET /api/Investidores/perfil/{perfil}` - Filtro por perfil de risco
- `GET /api/Investidores/faixa-etaria` - Filtro por faixa etária
- `GET /api/Investidores/saldo-minimo/{valor}` - Filtro por saldo mínimo
- `GET /api/Investidores/search/multiple` - Busca com múltiplos filtros
- `GET /api/Investidores/ordenados/nome` - Ordenados por nome
- `GET /api/Investidores/ordenados/saldo` - Ordenados por saldo
- `GET /api/Investidores/paginados` - Busca paginada
- `GET /api/Investidores/stats/media-idade` - Média de idade
- `GET /api/Investidores/stats/saldo-total` - Saldo total geral
- `GET /api/Investidores/stats/distribuicao-perfil` - Distribuição por perfil
- `GET /api/Investidores/stats/faixa-saldo` - Distribuição por faixa de saldo
- `GET /api/Investidores/top-saldos/{quantidade}` - Top investidores por saldo
- `GET /api/Investidores/nascidos-apos/{ano}` - Nascidos após determinado ano
- `GET /api/Investidores/ativos-recentes/{dias}` - Ativos nos últimos X dias
- `GET /api/Investidores/stats/resumo-completo` - Estatísticas completas

### 📈 Investimentos (Endpoints LINQ)
**CRUD Básico:**
- `GET /api/Investimentos` - Listar todos os investimentos
- `GET /api/Investimentos/{id}` - Buscar investimento por ID
- `GET /api/Investimentos/investidor/{investidorId}` - Listar por investidor
- `POST /api/Investimentos` - Criar novo investimento
- `PUT /api/Investimentos/{id}` - Atualizar investimento
- `DELETE /api/Investimentos/{id}` - Excluir investimento

**Análises Avançadas:**
- `GET /api/Investimentos/tipo/{tipo}` - Filtro por tipo
- `GET /api/Investimentos/rentabilidade-minima/{valor}` - Por rentabilidade
- `GET /api/Investimentos/periodo` - Por período específico
- `GET /api/Investimentos/status/{status}` - Por status
- `GET /api/Investimentos/stats/media-rentabilidade` - Média de rentabilidade
- `GET /api/Investimentos/stats/total-investido` - Total investido

### 🌐 APIs Externas

#### 📊 Alpha Vantage (6 Endpoints)
- `GET /api/alphavantage/quote/{symbol}` - Cotação atual de ação
- `GET /api/alphavantage/search/{keywords}` - Buscar símbolos
- `GET /api/alphavantage/daily/{symbol}` - Dados históricos diários
- `GET /api/alphavantage/intraday/{symbol}` - Dados intraday
- `GET /api/alphavantage/technical/{symbol}/{indicator}` - Indicadores técnicos
- `GET /api/alphavantage/news/{topics}` - Notícias financeiras

#### 📈 MarketStack (7 Endpoints Funcionais)
- `GET /api/marketstack/eod` - Dados End-of-Day ✅
- `GET /api/marketstack/eod/latest` - Dados mais recentes ✅
- `GET /api/marketstack/intraday` - Dados intraday ✅
- `GET /api/marketstack/intraday/latest` - Intraday mais recente ✅
- `GET /api/marketstack/tickers` - Lista de tickers ✅
- `GET /api/marketstack/exchanges` - Lista de bolsas ✅
- `GET /api/marketstack/dividends` - Histórico de dividendos ✅

### 📄 Arquivos
- `GET /api/Arquivos/exportar/investidores/json` - Exportar investidores para JSON
- `GET /api/Arquivos/exportar/investidores/txt` - Exportar investidores para TXT
- `POST /api/Arquivos/importar/investidores` - Importar investidores via JSON
- `GET /api/Arquivos/exportar/investimentos` - Exportar investimentos para JSON
- `POST /api/Arquivos/importar/investimentos` - Importar investimentos via JSON

## 📄 Exemplos de JSON para Testes

### Exemplo: Criar Investidor
```json
{
  "nome": "Maria Santos",
  "cpf": "123.456.789-01",
  "email": "maria.santos@email.com",
  "dataNascimento": "1985-03-20T00:00:00",
  "saldoTotal": 75000.00,
  "perfilRisco": "Arrojado"
}
```

### Exemplo: Criar Investimento
```json
{
  "nome": "CDB Banco XYZ",
  "tipo": "Renda Fixa",
  "valorInicial": 25000.00,
  "valorAtual": 26250.00,
  "rentabilidade": 5.0,
  "dataInicio": "2024-01-15T00:00:00",
  "dataVencimento": "2026-01-15T00:00:00",
  "investidorId": 1,
  "status": "Ativo"
}
```

## 🌐 Exemplos de Uso das APIs Externas

### Alpha Vantage - Cotação de Ações
```bash
GET /api/alphavantage/quote/AAPL
# Retorna: preço atual, variação, volume da Apple

GET /api/alphavantage/search/Apple
# Retorna: símbolos relacionados à Apple
```

### MarketStack - Dados de Mercado
```bash
GET /api/marketstack/eod/latest?symbols=AAPL,MSFT
# Retorna: dados de fim de dia para Apple e Microsoft

GET /api/marketstack/intraday/latest?symbols=AAPL&interval=1min
# Retorna: dados intraday em intervalos de 1 minuto

GET /api/marketstack/tickers?limit=10
# Retorna: lista dos primeiros 10 tickers disponíveis

GET /api/marketstack/exchanges
# Retorna: lista de todas as bolsas de valores
```

### Análises LINQ - Investidores
```bash
GET /api/Investidores/perfil/Arrojado
# Retorna: todos investidores com perfil arrojado

GET /api/Investidores/stats/distribuicao-perfil
# Retorna: quantos investidores por perfil de risco

GET /api/Investidores/search/multiple?nome=João&perfilRisco=Moderado
# Retorna: investidores chamados João com perfil moderado
```

### Exemplo: Múltiplos Investidores (para importação)
```json
[
  {
    "nome": "João Silva",
    "cpf": "111.111.111-11",
    "email": "joao@email.com",
    "dataNascimento": "1990-01-01T00:00:00",
    "saldoTotal": 50000.00,
    "perfilRisco": "Moderado"
  },
  {
    "nome": "Ana Costa",
    "cpf": "222.222.222-22",
    "email": "ana@email.com",
    "dataNascimento": "1992-05-10T00:00:00",
    "saldoTotal": 35000.00,
    "perfilRisco": "Conservador"
  }
]
```

## 🔍 Validações Implementadas

### Investidor
- Nome: obrigatório, máximo 100 caracteres
- CPF: obrigatório, máximo 14 caracteres
- Email: obrigatório, formato válido, máximo 100 caracteres
- Data de nascimento: obrigatória
- Saldo total: deve ser >= 0
- Perfil de risco: obrigatório, máximo 20 caracteres

### Investimento
- Nome: obrigatório, máximo 100 caracteres
- Tipo: obrigatório, máximo 50 caracteres
- Valor inicial: deve ser > 0
- Valor atual: deve ser >= 0
- Data de início: obrigatória
- Investidor ID: obrigatório, deve existir
- Status: obrigatório, máximo 20 caracteres

## 📁 Estrutura do Banco de Dados

### Tabela INVESTIDORES
```sql
CREATE TABLE INVESTIDORES (
    ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    NOME VARCHAR2(100) NOT NULL,
    CPF VARCHAR2(14) NOT NULL,
    EMAIL VARCHAR2(100) NOT NULL,
    DATANASCIMENTO DATE,
    SALDOTOTAL NUMBER(18,2),
    PERFILRISCO VARCHAR2(20)
);
```

### Tabela INVESTIMENTOS
```sql
CREATE TABLE INVESTIMENTOS (
    ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    NOME VARCHAR2(100) NOT NULL,
    TIPO VARCHAR2(50) NOT NULL,
    VALORINICIAL NUMBER(18,2) NOT NULL,
    VALORATUAL NUMBER(18,2) NOT NULL,
    RENTABILIDADE NUMBER(10,2),
    DATAINICIO DATE NOT NULL,
    DATAVENCIMENTO DATE,
    INVESTIDORID NUMBER NOT NULL,
    STATUS VARCHAR2(20) NOT NULL,
    CONSTRAINT FK_INVESTIDOR FOREIGN KEY (INVESTIDORID) REFERENCES INVESTIDORES(ID)
);
```

## 📁 Localização dos Arquivos

### Arquivos Exportados
Os arquivos exportados são salvos em:
```
InvestimentosApp/src/InvestimentosApp.API/Data/Exports/
├── investidores_[timestamp].json
├── investidores_[timestamp].txt
└── investimentos_[timestamp].json
```

### Arquivos de Exemplo para Testes
Na raiz do projeto você encontrará:
- `exemplos_investidores.json` - Dados de exemplo para importar investidores
- `exemplos_investimentos.json` - Dados de exemplo para importar investimentos

Para usar os exemplos:
1. Acesse o Swagger online: https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net/swagger/index.html
2. Vá para o endpoint `POST /api/Arquivos/importar/investidores`
3. Use o arquivo `exemplos_investidores.json` como teste

## ☁️ Infraestrutura e Deploy

### 🚀 **Azure Cloud**
- **App Service:** InveXpApp (East US 2)
- **Plano:** Free Tier (ideal para demonstração)
- **Runtime:** .NET 8.0 on Linux
- **URL:** https://invexpapp-b0fvc2e2eughdhd5.eastus2-01.azurewebsites.net

### 🔄 **CI/CD com GitHub Actions**
- **Deploy automático** a cada push na branch `master`
- **Build pipeline** com .NET 8
- **Deploy usando** Azure Web Apps Deploy
- **Logs** disponíveis no GitHub Actions

### ⚙️ **Configurações de Produção**
- **Variáveis de ambiente** seguras no Azure
- **API Keys** configuradas como Application Settings
- **Logs** integrados com Azure Monitor
- **HTTPS** automático com certificado SSL

### 📊 **Monitoramento**
- **Azure Application Insights** (opcional)
- **Logs em tempo real** via Azure Portal
- **Health checks** automáticos
- **Uptime monitoring** do Azure

## 🚀 Recursos Implementados

### 🔍 Consultas LINQ Avançadas
- **Filtros complexos** com múltiplos critérios
- **Agregações estatísticas** (média, soma, contagem)
- **Ordenação** por diferentes campos
- **Paginação** para grandes volumes de dados
- **Busca textual** em múltiplos campos
- **Análises temporais** por períodos

### 🌐 Integração com APIs Reais
- **Rate limiting** respeitando limites das APIs
- **Tratamento de erros** robusto
- **Cache inteligente** para reduzir chamadas
- **Logs detalhados** para debugging
- **Configuração flexível** via appsettings.json

### 📊 Dados de Mercado
- **170.000+ símbolos** de ações globais
- **70+ bolsas de valores** mundiais
- **Dados em tempo real** (planos pagos)
- **Histórico** de até 30 anos
- **Indicadores técnicos** profissionais
- **Notícias** de mercado atualizadas

## 🔐 Segurança e Boas Práticas

### ✅ Implementado
- **Validação de entrada** em todos os endpoints
- **Tratamento de exceções** padronizado
- **Logs estruturados** para monitoramento
- **Configuração externa** de APIs (não hardcoded)
- **Rate limiting** para APIs externas
- **Validação de CPF, email** e outros campos críticos

## 🎓 Grupo

- Gabriel ferla - RM550695
- Henri de Oliveira Lopes - RM98347
- Lorenzo Gomes Andreata - RM551117
- Lucas Moreno Matheus - RM97158
- Victor Flávio Demarchi Viana - RM99389
