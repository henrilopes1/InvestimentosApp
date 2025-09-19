# 🚀 Guia de Integração - Sistema de Gestão de Investimentos

## 📋 Pré-requisitos

- Visual Studio 2022 ou Visual Studio Code
- .NET 8.0 SDK instalado
- Acesso ao banco de dados Oracle (credenciais FIAP)

## 🔧 Configuração Inicial

### 1. Clone ou Download do Projeto
```bash
# Se estiver no Git
git clone [URL_DO_REPOSITORIO]

# Ou faça o download e extraia o arquivo ZIP
```

### 2. Abrir o Projeto
#### No Visual Studio 2022:
1. Abra o Visual Studio 2022
2. Clique em "Abrir um projeto ou solução"
3. Navegue até a pasta do projeto
4. Selecione o arquivo `InvestimentosApp.sln`

#### No Visual Studio Code:
1. Abra o Visual Studio Code
2. Use `Ctrl+Shift+P` e digite "File: Open Folder"
3. Selecione a pasta raiz do projeto

### 3. Restaurar Dependências
```bash
# Na pasta raiz do projeto
dotnet restore
```

## 🔑 Configuração do Banco de Dados

### Atualizar Credenciais Oracle
No arquivo `src/InvestimentosApp.API/Program.cs`, localize a linha:
```csharp
builder.Services.AddScoped<AppDbContext>(provider => 
    new AppDbContext("SEU_RM_AQUI", "SUA_SENHA"));
```

**Substitua pelos seus dados:**
- `"SEU_RM_AQUI"` → Seu RM da FIAP (ex: "RM12345")
- `"SUA_SENHA"` → Sua senha do Oracle

### String de Conexão
A conexão Oracle está configurada para:
- **Data Source**: `oracle.fiap.com.br:1521/ORCL`
- **Schema**: Usando suas credenciais FIAP

## ⚡ Executando o Projeto

### Via Terminal/CMD
```bash
# Navegue até a pasta da API
cd src/InvestimentosApp.API

# Execute o projeto
dotnet run
```

### Via Visual Studio
1. Defina `InvestimentosApp.API` como projeto de inicialização
2. Pressione `F5` ou clique em "Iniciar"

## 🌐 Acessando a Aplicação

Após executar o projeto, acesse:
- **Swagger UI**: http://localhost:5000/swagger/index.html

## 🧪 Testando a API

### 1. Via Swagger UI
1. Acesse http://localhost:5000/swagger/index.html
2. Teste os endpoints diretamente na interface
3. Use os exemplos fornecidos abaixo

## 📝 Exemplos de Uso

### Criar um Investidor
**POST** `/api/Investidores`
```json
{
  "nome": "João Silva",
  "cpf": "123.456.789-00",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-05-15T00:00:00",
  "saldoTotal": 50000.00,
  "perfilRisco": "Moderado"
}
```

### Criar um Investimento
**POST** `/api/Investimentos`
```json
{
  "nome": "Tesouro Direto IPCA+",
  "tipo": "Renda Fixa",
  "valorInicial": 10000.00,
  "valorAtual": 10500.00,
  "rentabilidade": 5.0,
  "dataInicio": "2024-01-01T00:00:00",
  "dataVencimento": "2029-01-01T00:00:00",
  "investidorId": 1,
  "status": "Ativo"
}
```

### Listar Investidores
**GET** `/api/Investidores`

### Exportar Dados
**GET** `/api/Arquivos/exportar/investidores/json`

## 🗄️ Estrutura do Banco de Dados

### Schema das Tabelas
```sql
-- INVESTIDORES
ID NUMBER (PK, Identity)
NOME VARCHAR2(100) NOT NULL
CPF VARCHAR2(14) NOT NULL  
EMAIL VARCHAR2(100) NOT NULL
DATANASCIMENTO DATE
SALDOTOTAL NUMBER(18,2)
PERFILRISCO VARCHAR2(20)

-- INVESTIMENTOS  
ID NUMBER (PK, Identity)
NOME VARCHAR2(100) NOT NULL
TIPO VARCHAR2(50) NOT NULL
VALORINICIAL NUMBER(18,2) NOT NULL
VALORATUAL NUMBER(18,2) NOT NULL
RENTABILIDADE NUMBER(10,2)
DATAINICIO DATE NOT NULL
DATAVENCIMENTO DATE
INVESTIDORID NUMBER NOT NULL (FK)
STATUS VARCHAR2(20) NOT NULL
```

## 🔍 Solução de Problemas

### Erro de Conexão Oracle
```
ORA-12170: TNS:Connect timeout occurred
```
**Solução**: Verifique se você está na rede FIAP ou usando VPN.

### Erro de Credenciais
```
ORA-01017: invalid username/password; logon denied
```
**Solução**: Verifique se o RM e senha estão corretos no `Program.cs`.

### ⚠️ IMPORTANTE: Criação das Tabelas
**As tabelas NÃO são criadas automaticamente** neste projeto. Você DEVE executar este script no Oracle SQL Developer ANTES de usar a API:
```sql
-- Criar tabela INVESTIDORES
CREATE TABLE INVESTIDORES (
    ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    NOME VARCHAR2(100) NOT NULL,
    CPF VARCHAR2(14) NOT NULL,
    EMAIL VARCHAR2(100) NOT NULL,
    DATANASCIMENTO DATE,
    SALDOTOTAL NUMBER(18,2),
    PERFILRISCO VARCHAR2(20)
);

-- Criar tabela INVESTIMENTOS
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

### Porta em Uso
Se a porta 5000 estiver ocupada, o sistema tentará automaticamente a 5001 (HTTPS).

### Packages NuGet Faltando
```bash
# Restore packages
dotnet restore

# Clean and rebuild
dotnet clean
dotnet build
```

## 📂 Estrutura de Arquivos

```
InvestimentosApp/
├── src/
│   ├── InvestimentosApp.Domain/     # Modelos e interfaces
│   ├── InvestimentosApp.Data/       # Repositórios e contexto
│   └── InvestimentosApp.API/        # Controllers e serviços
│       └── Data/Exports/            # Arquivos exportados
├── InvestimentosApp.sln             # Solução principal
├── README.md                        # Documentação principal
└── GUIA_INTEGRACAO.md              # Este guia
```

## 🎯 Próximos Passos

1. ✅ Configure suas credenciais Oracle
2. ✅ Execute o projeto
3. ✅ Teste via Swagger UI
4. ✅ Crie alguns dados de exemplo
5. ✅ Teste as funcionalidades de arquivo
6. ✅ Explore todos os endpoints

