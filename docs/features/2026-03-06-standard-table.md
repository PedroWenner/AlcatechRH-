# Feature: Standard Table

## Description
Componentizar as tabelas do sistema para garantir uma interface consistente, responsiva e de fácil manutenção. Atualmente, as tabelas em `Employees.tsx`, `Cargos.tsx` e `AuditLogs.tsx` possuem implementações manuais repetitivas.

## Requirements
- Componente `Table` genérico e tipado.
- Suporte a colunas dinâmicas (`header`, `render function`).
- Alinhamento de colunas configurável (`left`, `center`, `right`).
- Integração nativa com o componente de `Pagination`.
- Estilização padronizada (hover rows, header bg, border radius).
- Estado de visualização para "nenhum dado encontrado".

## Technical Details
- **Frontend**: Criar `src/components/common/Table.tsx`.
- **Pages**: Refatorar `Employees.tsx`, `Cargos.tsx` e `AuditLogs.tsx`.
