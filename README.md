# Desafio Cadastro (.NET + React)

Este repositório contém uma aplicação de exemplo para cadastro de pessoas com backend em .NET e frontend em React (Vite).

Este README explica como rodar o projeto localmente, executar testes, limpar o repositório de artefatos de build e onde encontrar endpoints importantes (Swagger).

Obs: Este projeto foi preparado para um processo seletivo — foram aplicadas boas práticas básicas (DTOs, validação de CPF, JWT para autenticação, testes de integração).

## Estrutura

- `src/Backend/CadastroPessoasApi/` - API em .NET (Models, Controllers, Data, Migrations)
- `src/Backend/CadastroPessoasApi.Tests/` - Projeto de testes xUnit (integração)
- `src/Frontend/cadastro-ui/` - Frontend React (Vite)
# Desafio Cadastro — Backend (.NET) + Frontend (React)

Este repositório contém uma implementação de referência para um sistema simples de cadastro de pessoas. O objetivo deste README é oferecer instruções claras para desenvolver, testar e publicar a aplicação localmente ou em um ambiente de produção simples.

Conteúdo:
- Backend: `src/Backend/CadastroPessoasApi/` (ASP.NET Core, EF Core, SQLite)
- Testes: `src/Backend/CadastroPessoasApi.Tests/` (xUnit, integração)
- Frontend: `src/Frontend/cadastro-ui/` (React + Vite)

Principais características implementadas
- Endpoints CRUD para Pessoa (com DTOs)
- Validações: CPF (algoritmo), email, data de nascimento e campos obrigatórios
- Armazenamento: SQLite (arquivo local) para desenvolvimento
- Autenticação: JWT (endpoint de login para obter token)
- Documentação: Swagger (com configuração para Bearer token)
- Testes de integração com WebApplicationFactory e SQLite in-memory

Requisitos
- .NET SDK 8.0+
- Node.js 18+ (ou LTS compatível)
- npm ou yarn

Configurações recomendadas (variáveis de ambiente)
- ASPNETCORE_ENVIRONMENT=Development | Production
- ConnectionStrings: a cadeia de conexão SQLite pode ser configurada em `appsettings.json` ou via variável de ambiente `ConnectionStrings__DefaultConnection`.
- JwtSettings: `JwtSettings__Key`, `JwtSettings__Issuer`, `JwtSettings__Audience` (para produção, use um segredo forte e seguro).

Rodando o backend (desenvolvimento)

1. Abrir PowerShell na raiz do repositório e executar:

```powershell
Push-Location .\src\Backend\CadastroPessoasApi
dotnet restore
dotnet build
```

2. (Opcional) Aplicar migrations para criar/atualizar o banco SQLite local:

```powershell
dotnet tool restore; dotnet ef database update
```

3. Iniciar a API:

```powershell
dotnet run
```

A API será exposta em `https://localhost:<porta>` (o console do `dotnet run` mostra a porta). A documentação interativa do Swagger fica em `/swagger`.

Autenticação e uso dos endpoints protegidos
- Endpoint de login: POST `/api/auth/login` — envie JSON { "username": "<user>", "password": "<pass>" } e receba `{ "token": "<JWT>" }`.
- Inclua o token no cabeçalho `Authorization: Bearer <token>` para acessar endpoints protegidos.

Desafio Cadastro (.NET + React)

Resumo curto e direto do que existe neste repositório:

- Backend: `src/Backend/CadastroPessoasApi/` — API ASP.NET Core com EF Core (SQLite).
- Testes: `src/Backend/CadastroPessoasApi.Tests/` — testes xUnit de integração.
- Frontend: `src/Frontend/cadastro-ui/` — app React + Vite (esqueleto).

O backend implementa endpoints básicos para gerenciamento de pessoas (CRUD). Há validação de CPF, email e campos obrigatórios. A API expõe documentação Swagger em ambiente de desenvolvimento.

Como executar (essencial)

1) Backend (PowerShell):

```powershell
Push-Location .\src\Backend\CadastroPessoasApi
dotnet restore
dotnet build
dotnet run
Pop-Location
```

Após `dotnet run`, acesse `https://localhost:<porta>/swagger` para a documentação.

2) Frontend (PowerShell):

```powershell
Push-Location .\src\Frontend\cadastro-ui
npm install
npm run dev
Pop-Location
```

3) Testes (PowerShell):

```powershell
Push-Location .\src\Backend\CadastroPessoasApi.Tests
dotnet test
Pop-Location
```

Observações técnicas mínimas

- A autenticação do backend usa JWT. Há um endpoint de login que emite token.
- O banco padrão para desenvolvimento é SQLite (arquivo local); os testes usam SQLite in-memory.
- Arquivos de build (`bin/` e `obj/`) devem ser ignorados pelo .gitignore.

Se precisar, eu faço apenas a alteração do README para remover ou ajustar linhas específicas sem outras mudanças.
