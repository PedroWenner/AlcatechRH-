# STATE MANAGEMENT

## Purpose

Definir como o estado da aplicação deve ser organizado.

---

## Principles

- Evitar espalhar estado por toda a aplicação  
- Preferir **centralização** em casos de compartilhamento global (Redux, Zustand, Context API)  
- Componentes isolados devem gerenciar apenas seu estado local  

---

## Guidelines

- Atualizações de estado devem ser **imutáveis**  
- Evitar re-renderizações desnecessárias  
- Utilizar selectors ou memoization para performance  
- Para formulários complexos, preferir bibliotecas como **React Hook Form**  