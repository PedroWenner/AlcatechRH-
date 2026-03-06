# QUERY OPTIMIZATION

## Purpose

Definir práticas para escrita de consultas SQL eficientes.

---

## Evitar SELECT *

Consultas devem retornar apenas colunas necessárias.

Exemplo correto:

SELECT id, nome, cpf

---

## Filtragem Antecipada

Filtros devem ser aplicados o mais cedo possível.

Utilizar WHERE corretamente evita leituras desnecessárias.

---

## Limitar Resultados

Consultas grandes devem utilizar LIMIT.

Isso evita carga excessiva no banco.

---

## Evitar Subconsultas Desnecessárias

Subqueries devem ser evitadas quando JOIN resolve o problema.

JOINs geralmente possuem melhor desempenho.

---

## Uso de EXPLAIN

Consultas complexas devem ser analisadas utilizando:

EXPLAIN  
EXPLAIN ANALYZE

Isso permite identificar gargalos de execução.