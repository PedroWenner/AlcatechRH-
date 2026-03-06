# Feature: Módulo de Permissões (RBAC)

## Status: Implementado ✅

## Descrição
Sistema de controle de acesso baseado em perfis (Role-Based Access Control) que permite permissões granulares tanto por módulo do sistema quanto por ações específicas dentro desses módulos.

## Requisitos
- Um usuário deve possuir um único perfil (ex: Admin, RH, Funcionário).
- Um perfil pode ter múltiplas permissões.
- As permissões seguem o padrão `Módulo:Ação` (ex: `Folha:Calcular`, `Funcionario:Listar`).
- O acesso deve ser validado tanto no Backend (via JWT Claims) quanto no Frontend (ocultando elementos de UI).

## Arquitetura Técnica

### Backend (C# .NET 9)
- **Entidades de Domínio**:
  - `Perfil`: Define o agrupamento de permissões.
  - `Permissao`: Define o par Módulo/Ação.
  - `PerfilPermissao`: Tabela de junção para relação muitos-para-muitos.
- **Segurança**:
  - O `AuthService` injeta as permissões no JWT como Claims do tipo `Permission`.
  - As permissões são carregadas via Eager Loading (`Include`) no momento do login.
- **Serviços**:
  - `IPerfilService` / `PerfilService` (Infrastructure): Gerencia a consulta de perfis e permissões.

### Frontend (React + Vite)
- **AuthContext / useAuth**: Hook customizado que decodifica o JWT e fornece funções auxiliares:
  - `hasPermission(modulo, acao)`: Verifica se o usuário tem uma ação específica.
  - `isAuthorized(modulo)`: Verifica se o usuário tem qualquer acesso ao módulo.
- **Componentes**:
  - `AppLayout`: Utiliza `isAuthorized` para renderizar condicionalmente os itens da Sidebar.

## Dados Iniciais (Seed)
- **Admin**: Acesso total (Folha:Acessar, Folha:Calcular, Funcionario:Listar, Funcionario:Editar).
- **RH**: Acesso operacional (Folha:Acessar, Funcionario:Listar). *Nota: RH não pode calcular folha por padrão neste seed.*
- **Funcionário**: Acesso restrito.

## Localização dos Arquivos
- **Entidades**: `DPManagement.Domain/Entities/`
- **Configurações EF**: `DPManagement.Infrastructure/Persistence/Configurations/`
- **Serviços**: `DPManagement.Infrastructure/Services/PerfilService.cs`
- **Hooks React**: `DPManagement.Web/src/hooks/useAuth.tsx`
