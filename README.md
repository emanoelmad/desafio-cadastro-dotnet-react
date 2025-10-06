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

Rodando o frontend (desenvolvimento)

```powershell
Push-Location .\src\Frontend\cadastro-ui
npm install
npm run dev
Pop-Location
```

O Vite geralmente expõe a aplicação em `http://localhost:5173`. Configure a URL da API no frontend conforme necessário (arquivo de ambiente ou variável de build).

Testes (xUnit)

Para executar os testes do backend:

```powershell
Push-Location .\src\Backend\CadastroPessoasApi.Tests
dotnet test
Pop-Location
```

Os testes de integração usam uma instância SQLite em memória e um handler de autenticação especial para evitar dependências externas durante a execução.

Swagger / Documentação da API

Ao rodar a API em ambiente de desenvolvimento, acesse `/swagger` para a documentação interativa. A configuração já inclui um esquema de segurança Bearer (JWT) para testar endpoints protegidos.

API versioning (nota)

O próximo passo planejado é disponibilizar versionamento de API (v1 e v2), onde v2 adicionará o campo `Endereco` como obrigatório. Atualmente a API expõe os endpoints principais em `/api/pessoas`.

Docker (opção rápida para produção)

É recomendado criar um `Dockerfile` para o backend e usar um serviço de hosting (ex.: Azure App Service, AWS ECS, DigitalOcean) para publicar a API. Um Dockerfile mínimo inclui:

- build com SDK
- publicação do app
- execução com `dotnet <Assembly>.dll`

Sugestão de CI/CD

- GitHub Actions ou Azure Pipelines para:
	- Build do backend
	- Execução dos testes (dotnet test)
	- Build e deploy do frontend (Vite)
	- Publicação do container (se usar Docker)

Boas práticas e dicas rápidas
- Não deixe segredos no repositório. Use o sistema de segredos da plataforma (GitHub Secrets, Azure Key Vault etc.).
- Configure variáveis de ambiente para connection strings e chaves JWT.
- Aumente a cobertura de testes adicionando testes unitários para o validador de CPF, para os DTOs e para os caminhos de falha dos controllers.

Problemas comuns e soluções
- Erro de porta / certificado ao rodar localmente: verifique o output do `dotnet run` para a porta e confie no certificado de desenvolvedor do ASP.NET Core (dotnet dev-certs https --trust).
- Arquivos `bin/` e `obj/` foram acidentalmente comitados: atualize o `.gitignore` e remova do índice com `git rm --cached -r -- src/**/bin src/**/obj`.

Contato / próximos passos
- Se quiser, eu implemento:
	- Versionamento da API (v1/v2) com DTOs separados e documentação Swagger por versão.
	- Frontend CRUD completo com autenticação via JWT.
	- Dockerfile e workflow GitHub Actions para build/test/deploy.

Obrigado — se preferir, posso ajustar este README para incluir exemplos de requests (cURL) específicos ou um Dockerfile de exemplo.
