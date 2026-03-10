# Feature: Bancos e Autocomplete

## Description
Provide a structure to store Banks (Bancos) with their COMPE codes and names, and integrate this into the newly created "Dados Bancários" feature to allow autocomplete for the "Código do Banco" field.

## Requirements
- Parse and seed data from `bancos.sql` (columns: `COMPE` -> `CodigoBanco`, `LongName` -> `Nome`, `ShortName` -> `NomeCurto`).
- Create `Banco` entity and REST endpoints for autocomplete searches (similar to the CBO autocomplete).
- Update the Frontend `DadosBancariosModal` to replace the simple input for "Código do Banco" with a searchable Select/Autocomplete component.
