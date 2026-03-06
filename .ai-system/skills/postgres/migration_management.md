# MIGRATION MANAGEMENT

## Purpose

Definir como alterações de banco devem ser versionadas.

---

## Versionamento

Toda alteração estrutural deve ser registrada como migration.

Exemplos:

- criação de tabela
- alteração de coluna
- criação de índice

---

## Reprodutibilidade

Uma migration deve poder ser executada em qualquer ambiente:

- desenvolvimento
- homologação
- produção

---

## Evitar Alterações Manuais

Alterações diretas no banco devem ser evitadas.

Toda mudança deve ocorrer via migration.