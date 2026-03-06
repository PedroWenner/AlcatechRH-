# ERROR HANDLING

## Purpose

Definir como erros devem ser tratados no frontend e padronizar a exibição de mensagens para o usuário.

---

## Principles

- Todos os erros que chegam do backend ou ocorrem no frontend **devem ser exibidos para o usuário**  
- Mensagens devem usar **SweetAlert, Toast ou outro padrão visual definido**  
- Evitar console.log ou alert nativo para mensagens de usuário final  
- Criar **hook ou componente centralizado** para gerenciar todos os alerts  

---

## Guidelines

- Usar try/catch em todas as funções async que interagem com APIs  
- Componentes devem chamar o hook central de erros para exibir mensagens  
- Diferenciar tipos de mensagens:  
  - Erro crítico → SweetAlert modal  
  - Sucesso → Toast pequeno  
  - Aviso → Toast ou modal leve  
- Não expor detalhes técnicos para o usuário; detalhes podem ser enviados para logging interno  
- Padrão de exibição deve ser **consistente em toda a aplicação**

---

## Implementation Suggestion

```jsx
import { useErrorHandler } from 'hooks/useErrorHandler'

const { showError } = useErrorHandler()

try {
  await api.get('/usuarios')
} catch (error) {
  showError(error) // exibe SweetAlert automaticamente
}