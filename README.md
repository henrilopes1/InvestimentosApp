# 📈 Sistema de Gestão de Investimentos

Um sistema completo de gestão de investimentos desenvolvido em C# com .NET 8, utilizando Oracle Database e seguindo arquitetura em camadas.

## 🎯 Funcionalidades Principais

### 👥 Gestão de Investidores
- ✅ Criar, listar, atualizar e excluir investidores
- ✅ Validações de CPF, email e campos obrigatórios
- ✅ Perfis de risco (Conservador, Moderado, Arrojado)
- ✅ Busca por ID e email

### 📈 Gestão de Investimentos
- ✅ Criar, listar, atualizar e excluir investimentos
- ✅ Tipos: Renda Fixa, Renda Variável, Fundos Imobiliários
- ✅ Controle de rentabilidade e valores
- ✅ Relacionamento com investidores
- ✅ Filtros por investidor

### 📄 Manipulação de Arquivos
- ✅ Exportação de investidores para JSON
- ✅ Importação de investidores via JSON
- ✅ Exportação de investimentos para JSON
- ✅ Importação de investimentos via JSON
- ✅ Exportação adicional para TXT
- ✅ Validações robustas nos arquivos importados

### 🌐 Interface Web API
- ✅ API RESTful completa com todos os endpoints
- ✅ Documentação interativa com Swagger UI
- ✅ Testes integrados na interface
- ✅ Tratamento de erros padronizado

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
│       ├── Services/                    # Lógica de aplicação
│       └── Program.cs                   # Configuração
└── InvestimentosApp.sln
```

**Camadas:**
- **Domain**: Entidades de negócio e interfaces
- **Data**: Repositórios e acesso ao Oracle Database
- **API**: Controllers, serviços e configuração da Web API

## 🛠️ Tecnologias Utilizadas

- **Framework**: .NET 8.0
- **Linguagem**: C# 12
- **Banco de Dados**: Oracle Database (FIAP)
- **ORM**: Entity Framework Core
- **Provedor Oracle**: Oracle.EntityFrameworkCore v7.21.12
- **Documentação API**: Swagger/OpenAPI (Swashbuckle.AspNetCore v6.5.0)
- **Arquitetura**: Clean Architecture (Camadas)
- **Padrões**: Repository Pattern, Dependency Injection

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code
- Acesso ao banco Oracle SQL Developer

### Configuração
1. Clone o repositório
2. Configure suas credenciais Oracle no `Program.cs`:
   ```csharp
   builder.Services.AddScoped<AppDbContext>(provider => 
       new AppDbContext("SEU_RM", "SUA_SENHA"));
   ```
3. Execute as migrações (**IMPORTANTE**: As tabelas devem ser criadas manualmente - veja script SQL abaixo)

### Execução
```bash
cd src/InvestimentosApp.API
dotnet run
```

### Acesso
- **Swagger UI**: http://localhost:5000/swagger/index.html

## 📊 Endpoints da API

### Investidores
- `GET /api/Investidores` - Listar todos os investidores
- `GET /api/Investidores/{id}` - Buscar investidor por ID
- `POST /api/Investidores` - Criar novo investidor
- `PUT /api/Investidores/{id}` - Atualizar investidor
- `DELETE /api/Investidores/{id}` - Excluir investidor

### Investimentos
- `GET /api/Investimentos` - Listar todos os investimentos
- `GET /api/Investimentos/{id}` - Buscar investimento por ID
- `GET /api/Investimentos/investidor/{investidorId}` - Listar por investidor
- `POST /api/Investimentos` - Criar novo investimento
- `PUT /api/Investimentos/{id}` - Atualizar investimento
- `DELETE /api/Investimentos/{id}` - Excluir investimento

### Arquivos
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
1. Acesse o Swagger em http://localhost:5000/swagger
2. Vá para o endpoint `POST /api/Arquivos/importar/investidores`
3. Use o arquivo `exemplos_investidores.json` como teste

## 🎓 Grupo

Gabriel ferla - RM550695
Henri de Oliveira Lopes - RM98347
Lorenzo Gomes Andreata - RM551117
Lucas Moreno Matheus - RM97158
Victor Flávio Demarchi Viana - RM99389
