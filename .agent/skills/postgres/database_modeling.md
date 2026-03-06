# DATABASE MODELING

## Purpose

Definir boas práticas para modelagem de dados em PostgreSQL.

O objetivo é garantir:

- integridade de dados
- organização clara
- manutenção simples
- consistência estrutural

---

## Modelagem Relacional

A modelagem deve seguir princípios relacionais.

Cada entidade deve possuir:

- chave primária
- atributos bem definidos
- relacionamentos explícitos

---

## Normalização

As tabelas devem seguir até **3ª forma normal** sempre que possível.

Isso evita:

- redundância de dados
- inconsistências
- dificuldades de manutenção

---

## Chave Primária

Toda tabela deve possuir uma chave primária.

Padrão:

id SERIAL ou id BIGSERIAL

---

## Chaves Estrangeiras

Relacionamentos entre tabelas devem utilizar **foreign keys**.

Isso garante integridade referencial entre registros.

---

## Tipos de Dados

Os tipos devem ser escolhidos corretamente.

Exemplos:

INTEGER para números inteiros  
NUMERIC para valores monetários  
VARCHAR para textos curtos  
TEXT para textos longos  
DATE para datas  
TIMESTAMP para data e hora

---

## Evitar Colunas Genéricas

Colunas genéricas devem ser evitadas.

Exemplo ruim:

dados  
informacao  
valor1

As colunas devem ser descritivas.