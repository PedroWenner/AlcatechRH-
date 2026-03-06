# POSTGRES PERFORMANCE GUIDELINES

## Evitar Consultas Pesadas

Consultas que retornam grandes volumes devem ser paginadas.

---

## Monitoramento

O banco deve ser monitorado para identificar:

- queries lentas
- locks
- consumo de recursos

---

## Índices Corretos

Consultas críticas devem possuir índices adequados.

---

## Manutenção

Executar periodicamente:

VACUUM  
ANALYZE

Isso mantém estatísticas atualizadas.