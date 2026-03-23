# Feature: Restrição de Exclusão por Vínculos Ativos

## Descrição
Implementação de travas de segurança para impedir a exclusão de Órgãos e Centros de Custos que possuam vínculos com colaboradores (Vínculos). Isso garante a integridade referencial e evita órfãos no sistema.

## Regras de Negócio
- Um **Órgão** não pode ser excluído se ele ou qualquer um de seus descendentes (Secretarias, Departamentos) possuir um Vínculo ativo.
- Um **Centro de Custo** não pode ser excluído se possuir um Vínculo ativo.

## Implementação Técnica
- No `OrgaoService.RemoverAsync`, realizar uma verificação recursiva na tabela de `Vinculos` antes de proceder com o `soft delete`.
- No `CentroCustoService.RemoverAsync`, verificar a existência de registros na tabela de `Vinculos` para o `Id` informado.
