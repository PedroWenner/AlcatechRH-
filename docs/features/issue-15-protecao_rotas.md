# Feature: Proteção de Rotas (Route Guard)

## Status: Implementado ✅

## Descrição
Implementação de proteção de rotas no Frontend para garantir que apenas usuários autenticados (com token válido) acessem as áreas internas do sistema.

## Requisitos
- Redirecionar usuários não autenticados de `/` (ou qualquer rota protegida) para `/login`.
- Impedir que usuários logados acessem a página de `/login`, redirecionando-os para o Dashboard.
- Garantir que a verificação seja feita em um componente de alto nível (Layout) para cobrir todas as rotas filhas.

## Implementação

### AppLayout.tsx
- Adicionado `useEffect` que monitora o `token` do `useAuth`.
- Caso o `token` seja nulo, utiliza o `useNavigate` para enviar o usuário para `/login`.
- Bloqueia a renderização do conteúdo (`return null`) enquanto o redirecionamento ocorre.

### Login.tsx
- Adicionado `useEffect` que verifica se o usuário já possui um `token`.
- Se sim, redireciona para a raiz `/` para evitar login duplo.

## Localização dos Arquivos
- **Layout Principal**: [AppLayout.tsx](file:///d:/Faculdade/AlcatechRH/DPManagement.Web/src/components/AppLayout.tsx)
- **Página de Login**: [Login.tsx](file:///d:/Faculdade/AlcatechRH/DPManagement.Web/src/pages/Login.tsx)
