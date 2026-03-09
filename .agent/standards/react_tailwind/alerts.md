# Alerts Standards

## Purpose

Padronizar o uso de alertas no frontend utilizando **SweetAlert2**.

Todos os alertas do sistema devem seguir um padrão visual e comportamental consistente para garantir:

- melhor experiência do usuário
- feedback claro das ações
- padronização entre telas
- reutilização de código

---

# Alert Library

Biblioteca obrigatória:


sweetalert2


Importação padrão:

```typescript
import Swal from 'sweetalert2'
General Rules

Todos os alertas devem seguir estas regras:

❌ Nunca usar alert() nativo do navegador

❌ Nunca criar modais customizados para confirmação simples

✔ Sempre usar SweetAlert2

✔ Sempre possuir título claro

✔ Mensagem objetiva

✔ Ícone apropriado

✔ Botões consistentes

Alert Types
Success Alert

Usado quando uma operação foi concluída com sucesso.

Exemplos:

cadastro realizado

atualização concluída

operação finalizada

Swal.fire({
  icon: 'success',
  title: 'Sucesso',
  text: 'Operação realizada com sucesso'
})
Error Alert

Usado para erros inesperados ou falhas de operação.

Exemplos:

erro na API

erro de processamento

falha ao salvar dados

Swal.fire({
  icon: 'error',
  title: 'Erro',
  text: 'Ocorreu um erro ao realizar a operação'
})
Warning Alert

Usado para avisos importantes antes de uma ação crítica.

Exemplos:

alteração irreversível

dados incompletos

validações importantes

Swal.fire({
  icon: 'warning',
  title: 'Atenção',
  text: 'Verifique as informações antes de continuar'
})
Confirmation Alert

Toda ação destrutiva deve solicitar confirmação.

Exemplos:

excluir registro

cancelar processo

remover usuário

apagar arquivo

Exemplo padrão
import Swal from 'sweetalert2'

Swal.fire({
  title: 'Tem certeza?',
  text: 'Esta ação não poderá ser desfeita.',
  icon: 'warning',
  showCancelButton: true,
  confirmButtonText: 'Sim, continuar',
  cancelButtonText: 'Cancelar',
  confirmButtonColor: '#ef4444',
  cancelButtonColor: '#6b7280'
}).then((result) => {

  if (result.isConfirmed) {

    Swal.fire({
      icon: 'success',
      title: 'Concluído',
      text: 'Operação realizada com sucesso'
    })

  }

})
Delete Confirmation Pattern

Todas as exclusões devem seguir este padrão.

Swal.fire({
  title: 'Excluir registro?',
  text: 'Esta ação não poderá ser desfeita.',
  icon: 'warning',
  showCancelButton: true,
  confirmButtonText: 'Sim, excluir',
  cancelButtonText: 'Cancelar',
  confirmButtonColor: '#ef4444'
})
Async Operation Pattern

Quando a ação envolver chamada de API.

const result = await Swal.fire({
  title: 'Excluir registro?',
  text: 'Esta ação não poderá ser desfeita.',
  icon: 'warning',
  showCancelButton: true,
  confirmButtonText: 'Sim, excluir'
})

if (result.isConfirmed) {

  await api.delete('/registro/1')

  Swal.fire({
    icon: 'success',
    title: 'Registro excluído'
  })

}
Loading Alert

Para operações demoradas.

Swal.fire({
  title: 'Processando...',
  allowOutsideClick: false,
  didOpen: () => {
    Swal.showLoading()
  }
})

Encerramento:

Swal.close()
API Error Pattern

Quando erro vem da API.

Swal.fire({
  icon: 'error',
  title: 'Erro',
  text: error.response?.data?.message || 'Erro inesperado'
})
Form Validation Error

Quando formulário contém erros.

Swal.fire({
  icon: 'warning',
  title: 'Campos inválidos',
  text: 'Verifique os campos obrigatórios'
})
Toast Notifications (Optional)

Para notificações rápidas de sucesso ou erro leve, o sistema foi atualizado para utilizar o `react-hot-toast` ao invés do `SweetAlert2` (embora o SweetAlert ainda seja utilizado para modais de confirmação e warnings pesados).

```typescript
import toast from 'react-hot-toast';

toast.success('Salvo com sucesso');
toast.error('Erro de gravação');
```
UX Rules

Alertas devem:

✔ possuir mensagem clara
✔ ser utilizados apenas quando necessário
✔ não interromper fluxo desnecessariamente

Evitar:

❌ excesso de alertas
❌ alertas para informações triviais
❌ bloquear interação sem necessidade

Reusable Alert Service (Recommended)

Criar um helper centralizado.

Arquivo sugerido:

services/alertService.ts

Exemplo:

import Swal from 'sweetalert2'

export const alertSuccess = (message: string) => {
  Swal.fire({
    icon: 'success',
    title: 'Sucesso',
    text: message
  })
}

export const alertError = (message: string) => {
  Swal.fire({
    icon: 'error',
    title: 'Erro',
    text: message
  })
}

export const alertConfirm = async (message: string) => {

  const result = await Swal.fire({
    title: 'Confirmação',
    text: message,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Confirmar',
    cancelButtonText: 'Cancelar'
  })

  return result.isConfirmed
}

Uso:

const confirmed = await alertConfirm('Deseja excluir este registro?')

if (confirmed) {
  // executar ação
}
Summary

Todos os alertas devem:

✔ usar SweetAlert2
✔ possuir padrão visual consistente
✔ centralizar lógica quando possível
✔ seguir padrões de confirmação para ações destrutivas
✔ fornecer feedback claro ao usuário


---

💡 **Sugestão importante para seu workspace de IA**

Vale muito a pena criar também:


standards/frontend/api_response_handling.md


Porque isso permite padronizar algo essencial:

- **erros da API → SweetAlert**
- **sucesso → toast**
- **validação → erro de formulário**

Isso evita que cada dev trate resposta da API de forma diferente.  

Se quiser, eu posso criar esse **standard de resposta de API que conecta backend C# + React + SweetAlert** (fica extremamente poderoso no seu projeto).