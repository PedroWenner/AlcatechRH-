# Feature: Standard Table Filters

## Description
Adicionar filtros de consulta para todas as tabelas do sistema, permitindo que os usuários busquem registros específicos de forma eficiente.

## Requirements
- Filtros no backend integrados à paginação.
- Interface de filtros expansível/moderna no frontend.
- Filtros mínimos por entidade:
    - **Colaboradores**: Nome, CPF, Cargo.
    - **Cargos**: Nome, CBO.

## Technical Details
- **Backend**: Update `GetPagedAsync` or create `GetFilteredPagedAsync` in repositories.
- **Frontend**: Create a `FilterBar` component to handle query states.
- **Performance**: Ensure filters are applied at the database level (IQueryable).
