# Feature: Módulo de Colaboradores e Cargos

## Descrição
Criação dos módulos de Colaboradores e Cargos, atendendo aos requisitos da legislação (eSocial), com validações de CPF, máscaras padronizadas e consulta automática de CEP.

## Critérios de Aceite
- Cadastro de Cargos (Nome, CBO).
- Cadastro de Colaboradores com todos os campos obrigatórios.
- Validação de CPF.
- Máscaras: Telefone `(99) 0000-0000`, Celular `(99) 0 0000-0000`, CPF `555.555.555-55`, CEP `00000-000`.
- Consulta de CEP via API (ViaCEP) com fallback para edição manual.

## Detalhes Técnicos
- Branch: `feature/issue-01-modulos-colaboradores-cargos`
- Localização: `docs/features/2026-03-06-colaboradores-cargos.md`
