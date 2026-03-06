# Button Standards

## Purpose

Padronizar botões utilizados no sistema garantindo:

- consistência visual
- melhor experiência do usuário
- reutilização de componentes
- previsibilidade de ações

Todos os botões devem ser implementados utilizando **React + TailwindCSS**.

---

# General Rules

Todos os botões devem:

✔ possuir tamanho consistente  
✔ possuir espaçamento interno padrão  
✔ possuir ícone quando aplicável  
✔ possuir estado hover  
✔ possuir estado disabled  
✔ possuir estado loading quando necessário  

Nunca:

❌ criar estilos inline  
❌ duplicar estilos em cada tela  
❌ usar cores diferentes das definidas no padrão  

---

# Default Button Style

Todos os botões devem usar o seguinte padrão base:

```jsx
className="inline-flex items-center gap-2 rounded-lg px-4 py-2 text-sm font-medium transition"