# TRANSACTION HANDLING

## Purpose

Definir boas práticas para uso de transações.

---

## Atomicidade

Operações relacionadas devem ocorrer dentro da mesma transação.

Isso garante consistência dos dados.

---

## Uso de BEGIN / COMMIT

Fluxo padrão:

BEGIN  
executar operações  
COMMIT

Em caso de erro:

ROLLBACK

---

## Transações Curtas

Transações devem ser curtas.

Transações longas podem gerar:

- locks
- bloqueios de registros
- perda de performance