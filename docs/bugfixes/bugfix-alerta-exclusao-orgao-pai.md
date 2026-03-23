# Bugfix: Alerta de Exclusão de Órgãos com Filhos

## Descrição
Ao excluir um órgão que possui órgãos subordinados (Secretarias ou Departamentos), o usuário deve ser alertado sobre a quantidade de registros que serão removidos em cascata.

## Requisitos
- Backend: Endpoint para retornar a contagem de descendentes.
- Backend: Garantir que a exclusão lógica seja recursiva (soft delete para filhos e netos).
- Frontend: Exibir alerta específico listando a quantidade de estruturas vinculadas antes de confirmar a exclusão.

## Solução Técnica
1. Adicionar `ObterContagemDescendentesAsync` ao `OrgaoService`.
2. Refatorar `RemoverAsync` no `OrgaoService` para percorrer a árvore de descendentes.
3. Chamar o novo endpoint no frontend `Orgaos.tsx` antes de exibir o `alertDeleteConfirm`.
