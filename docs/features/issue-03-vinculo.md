# Resumo da Requisição

**Origem:** Task Router Workflow
**Tipo:** Feature
**Título:** Criar Rotina de Cadastro de Vínculos (Contratos / Matrículas)
**Descrição:** Implementar a entidade `Vinculo` no sistema. O vínculo representa a "folha de contratação" ou o contrato ativo de um colaborador com a instituição. Um colaborador pode ter mais de um vínculo ao longo do tempo ou simultaneamente (ex: dois cargos de professor, etc).

## Entidade de Domínio (`Vinculo`)
Os campos obrigatórios do formulário solicitados são:
- `ColaboradorId` (Relacionamento com a entidade `Colaborador`)
- `OrgaoId` (Relacionamento com a entidade `Orgao`)
- `Matricula` (String, identificador funcional único por órgão)
- `CargoId` (Relacionamento com a entidade `Cargo`)
- `RegimeJuridicoId` / `TipoVinculo` (Enum)
- `FormaIngressoId` (Enum)
- `CentroCustoId` (Relacionamento com a entidade `CentroCusto`)
- `DataAdmissao` (Date/DateTime)

*Observação Técnica:* O sistema já possui a estrutura para consultar todos esses relacionamentos no *Frontend* via `Autocomplete` ou listas (Órgãos, Cargos, Centros de Custo, Colaboradores e a recém-criada rota genérica de Enum para Regime e Forma de Ingresso).

## Pipeline de Resolução

**1. DEV Specialist (Backend)**
- Mapear a entidade `Vinculo` (`DPManagement.Domain/Entities`).
- Criar migração EF Core para gerar a tabela `Vinculos` (com chaves estrangeiras apropriadas).
- Implementar `IVinculoService` e `VinculoService` (`DPManagement.Infrastructure`).
- Implementar `VinculosController` para CRUD (`DPManagement.API`).

**2. FRONTEND Specialist (React)**
- Criar a view de listagem `src/pages/Vinculos.tsx` com `FilterBar` (Busca por matrícula ou nome do colaborador) e a `<Table>`.
- Adicionar o menu lateral de **Vínculos** no `AppLayout.tsx`.
- Criar o `<Modal>` de Cadastro padronizado (`modals.md`) contendo as chamadas de busca (`Autocomplete`) para os selects relacionais (Colaboradores, Órgãos, Cargos, CentroCustos) e usando o novo `<EnumSelect>` de Regimes e Ingressos.
- Atrelar eventos de `alertService` (Sucesso/Erro).
- Implementar `DatePicker` para a data de admissão.

**3. QA / Product Owner**
- Validar se o salvamento ocorre perfeitamente e exigir explicitamente o Merge.
