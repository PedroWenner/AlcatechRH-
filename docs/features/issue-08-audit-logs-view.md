# Feature: Consulta de Logs de Auditoria

## Descrição
Implementação de uma tela para consulta do histórico de alterações do sistema, permitindo rastrear quem alterou o quê e quando.

## Requisitos
- Listagem paginada dos logs de auditoria.
- Filtros por:
    - Período (Data Início e Data Fim).
    - Usuário (Quem realizou a alteração).
    - Tabela (Qual entidade foi alterada).
    - Ação (Insert, Update, Delete).
- Visualização dos detalhes da alteração (Valores Antigos vs Novos).

## Detalhes Técnicos
- **Endpoint**: `GET /api/audit-logs`
- **Filtros**: Parâmetros de query string.
- **Frontend**: Nova página `AuditLogs.tsx` no dashboard.
