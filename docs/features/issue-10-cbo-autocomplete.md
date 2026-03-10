# Feature: CBO Autocomplete

## Description
Implementar um campo de autocomplete para a Classificação Brasileira de Ocupações (CBO) no cadastro de cargos. Isso permitirá que os usuários selecionem o código CBO oficial a partir de uma lista pré-carregada de mais de 2.000 títulos.

## Requirements
- Fonte de dados: `cbo2002.csv`.
- Busca no backend por código ou título.
- Componente de Autocomplete no frontend reutilizável.
- Tratamento de encoding `Windows-1252` para caracteres especiais.

## Technical Details
- **Entidade**: `Cbo` com campos `Codigo` e `Titulo`.
- **Ingestão**: Script de seeding que lê o CSV no diretório público do webapp (ou movido para o backend).
- **Frontend**: Componente `Autocomplete` usando Tailwind CSS.
