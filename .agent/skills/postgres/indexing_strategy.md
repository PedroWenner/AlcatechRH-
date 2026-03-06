# INDEXING STRATEGY

## Purpose

Definir estratégias corretas de indexação.

---

## Quando Criar Índices

Índices devem ser criados para:

- chaves estrangeiras
- colunas usadas em filtros
- colunas usadas em JOIN
- colunas usadas em ORDER BY

---

## Evitar Índices Excessivos

Índices em excesso podem prejudicar:

- inserts
- updates
- deletes

Criar apenas índices necessários.

---

## Índices Compostos

Quando consultas utilizam múltiplas colunas em filtros, índices compostos devem ser considerados.

Exemplo:

(data_admissao, id_empresa)

---

## Monitoramento

Índices devem ser monitorados periodicamente para identificar:

- índices não utilizados
- índices redundantes