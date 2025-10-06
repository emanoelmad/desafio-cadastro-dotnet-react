# Desafio Cadastro (.NET + React)

Este repositório contém uma aplicação de exemplo para cadastro de pessoas com backend em .NET e frontend em React (Vite).

Este README explica como rodar o projeto localmente, executar testes, limpar o repositório de artefatos de build e onde encontrar endpoints importantes (Swagger).

Obs: Este projeto foi preparado para um processo seletivo — foram aplicadas boas práticas básicas (DTOs, validação de CPF, JWT para autenticação, testes de integração).

## Estrutura

- `src/Backend/CadastroPessoasApi/` - API em .NET (Models, Controllers, Data, Migrations)
- `src/Backend/CadastroPessoasApi.Tests/` - Projeto de testes xUnit (integração)
- `src/Frontend/cadastro-ui/` - Frontend React (Vite)

## Requisitos

- .NET SDK 8.0+
- Node.js 18+ (ou LTS atual)
- npm (ou yarn)

## Como rodar localmente (backend)

Abra um PowerShell e execute:

```powershell
# Navegar até a pasta do backend
Push-Location .\src\Backend\CadastroPessoasApi

# Restaurar dependências e compilar
dotnet restore
dotnet build

# Aplicar migrations (cria banco sqlite local `cadastro.db`)
# Se não quiser aplicar, ignore este passo
dotnet tool restore || $true
dotnet ef database update || $true

# Iniciar a API
dotnet run

# Depois de terminar, volte
Pop-Location
```

A API será exposta em `https://localhost:<porta>` (o console do `dotnet run` mostra a porta). O Swagger estará disponível em `https://localhost:<porta>/swagger`.

Usuários de teste (para obter JWT via `/api/auth/login`):
- `admin` / `password123`
- `user` / `password123`

Exemplo de login (curl):

```bash
curl -X POST "https://localhost:5001/api/auth/login" -H "Content-Type: application/json" -d '{"username":"admin","password":"password123"}'
```

Use o token retornado no header `Authorization: Bearer <token>` para acessar os endpoints protegidos.

## Como rodar o frontend (desenvolvimento)

Abra outro terminal e execute:

```powershell
Push-Location .\src\Frontend\cadastro-ui
npm install
npm run dev
Pop-Location
```

O Vite normalmente expõe a app em `http://localhost:5173`.

## Testes (xUnit)

Para executar os testes de integração/uniários:

```powershell
Push-Location .\src\Backend\CadastroPessoasApi.Tests
dotnet test
Pop-Location
```

Os testes usam uma instância in-memory de SQLite e um handler de autenticação de teste para evitar dependências externas.

## Limpar índice Git (remover bin/obj do repositório)

Se porventura artefatos de build foram adicionados ao repositório, há um script auxiliar que remove esses arquivos do índice (`git rm --cached`) sem deletá-los do disco e commita a mudança.

Executar (no root do repositório):

```powershell
.\scripts\clean-git-index.ps1
```

Verifique o `git status` após rodar o script e faça o push.

## Scripts úteis
- `scripts/run-backend.ps1` — restaura, aplica migrações (opcional) e roda o backend.
- `scripts/run-frontend.ps1` — instala dependências e roda o frontend Vite.
- `scripts/clean-git-index.ps1` — remove `bin/obj/.vs` do índice e commita a alteração.

## Endpoints principais
- POST `/api/auth/login` — recebe `{ username, password }`, retorna `{ token }`.
- GET `/api/pessoas` — lista pessoas (protegido por JWT).
- POST `/api/pessoas` — cria pessoa (valida CPF).
- PUT `/api/pessoas/{id}` — atualiza pessoa.
- DELETE `/api/pessoas/{id}` — remove pessoa.

## Boas práticas e observações para avaliação
- Não inclua secrets no repositório; use variáveis de ambiente em produção.
- Habilite o XML docs se quiser melhorar o Swagger (`<GenerateDocumentationFile>true</GenerateDocumentationFile>` no csproj) e adicione comentários aos controllers.
- Para um deploy rápido: criar Dockerfile para o backend e usar Vercel/Netlify para o frontend.

---

Se quiser, eu crio também um `Dockerfile` para o backend e um workflow GitHub Actions que roda build + testes e publica o frontend automaticamente.
