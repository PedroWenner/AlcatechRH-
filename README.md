# AlcatechRH — DP Management

Sistema de **Departamento Pessoal (DP)** para gestão de colaboradores, vínculos, cargos, rubricas de folha e estrutura organizacional, com foco em aderência ao **eSocial** (ex.: evento S-1010 — Tabela de Rubricas e Tabela 03 de Natureza).

> Projeto de portfólio — API em **.NET 9** (Clean Architecture) + SPA em **React 19 / TypeScript**.

## Funcionalidades

- **Colaboradores e vínculos** — cadastro, salário base, regras de restrição de exclusão
- **Cargos e CBO** — autocomplete com a base CBO 2002
- **Rubricas** — cadastro com natureza (Tabela 03 eSocial) e rotina de cálculo
- **Estrutura organizacional** — órgãos, secretarias, departamentos e centros de custo
- **Dados bancários** — com carga da tabela de bancos
- **Segurança** — autenticação JWT, perfis e permissões por módulo, proteção de rotas, expiração de token
- **Auditoria** — log de alterações com tela de consulta
- **UI padronizada** — tabelas com filtros e paginação, modais e datepickers reutilizáveis

## Arquitetura

```
DPManagement.Domain          Entidades, enums, interfaces de repositório
DPManagement.Application     Serviços, DTOs, validadores (regras de negócio)
DPManagement.Infrastructure  EF Core + PostgreSQL, migrations, repositórios
DPManagement.API             Controllers REST, JWT, Swagger, middleware de erros
DPManagement.Web             Front-end React + Vite + Tailwind CSS
docs/                        Especificações de features, bugfixes e padrões
```

Decisões: separação em camadas (Clean Architecture), padrão Repository, configurações do `DbContext` em arquivos individuais (`IEntityTypeConfiguration`), padrão único de resposta da API ([docs/standards/api-response-pattern.md](docs/standards/api-response-pattern.md)) e tratamento global de exceções.

## Stack

| Camada | Tecnologias |
| --- | --- |
| Back-end | .NET 9, ASP.NET Core, EF Core 9, Npgsql, JWT Bearer, BCrypt, Swagger |
| Banco | PostgreSQL |
| Front-end | React 19, TypeScript, Vite, Tailwind CSS 4, React Router 7, Axios |
| CI | GitHub Actions (build back-end e front-end) |

## Como executar

**Pré-requisitos:** .NET SDK 9, Node.js 22+, PostgreSQL.

```bash
# 1. Configuração local da API
cp DPManagement.API/appsettings.Development.example.json DPManagement.API/appsettings.Development.json
# edite a connection string e a chave JWT

# 2. Banco e API (aplica as migrations)
dotnet ef database update --project DPManagement.Infrastructure --startup-project DPManagement.API
dotnet run --project DPManagement.API        # http://localhost:5286 (Swagger em /swagger)

# 3. Front-end
cd DPManagement.Web
npm ci
npm run dev
```

Usuário administrador inicial (seed): `admin@dpmanagement.com` — veja [UsuarioConfiguration.cs](DPManagement.Infrastructure/Persistence/Configurations/UsuarioConfiguration.cs). Use apenas em desenvolvimento.

## Documentação

Cada funcionalidade possui especificação em [docs/features](docs/features) e correções em [docs/bugfixes](docs/bugfixes).

## Autor

**Pedro Wenner** — [GitHub](https://github.com/PedroWenner)

Licença [MIT](LICENSE).
