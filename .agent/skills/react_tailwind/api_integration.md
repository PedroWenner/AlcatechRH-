# API INTEGRATION

## Purpose

Definir práticas para consumo de APIs RESTful no frontend React.

---

## Principles

- Criar **services** separados para chamadas HTTP  
- Evitar lógica de API dentro de componentes  
- Tratar estados de **loading, success e error**  

---

## Guidelines

- Utilizar fetch ou axios com configuração centralizada  
- Implementar tratamento de erros consistente  
- Retornar dados já transformados para os componentes  
- Configurar headers, tokens e autenticação centralmente  