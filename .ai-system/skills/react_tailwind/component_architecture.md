
---

# skills/react_tailwind/component_architecture.md (atualizado)

```md id="cfa8b1"
# COMPONENT ARCHITECTURE

## Purpose

Definir como os componentes React devem ser estruturados e organizados para **reutilização e manutenção**.

---

## Principles

- Cada componente deve ter **uma única responsabilidade**  
- Componentes devem ser **modulares e reutilizáveis**  
- Evitar lógica complexa dentro de componentes de UI  
- Separar lógica de negócios em hooks ou serviços  
- **Grandes blocos de UI (cards, dashboards, tabelas, formulários grandes) devem ser componentizados obrigatoriamente**  

---

## Folder Structure

- components/ → componentes reutilizáveis  
- pages/ → páginas e views  
- layouts/ → estruturas de página e templates  
- hooks/ → lógica reutilizável  
- services/ → integração com APIs ou lógica de negócio  

---

## Props & State

- Props devem ser **claras e descritivas**  
- Evitar passar muitos props para um mesmo componente  
- Componentes de UI devem ser majoritariamente **stateless**  
- Componentes stateful devem centralizar o estado quando necessário  

---

## Guidelines for Large Blocks

- Dividir dashboards, tabelas grandes e formulários complexos em **subcomponentes menores**  
- Criar pastas próprias para componentes de grande porte, ex:  

components/Dashboard/
CardResumo.jsx
GraficoVendas.jsx
TabelaFuncionarios.jsx

- Reutilizar esses componentes em múltiplas páginas quando possível  
- Evitar colocar JSX longo diretamente dentro das páginas  

---

## Benefits

- Facilita manutenção e testes unitários  
- Reduz duplicação de código  
- Garante consistência visual e comportamento previsível 