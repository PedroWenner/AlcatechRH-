# FORM VALIDATION STANDARD

## Purpose

Padronizar validações de formulários em runtime utilizando **React + TailwindCSS**, garantindo:

- UX consistente
- feedback visual imediato
- mensagens de erro claras
- reutilização de componentes
- validação antes de envio para API

---

# Validation Strategy

Toda validação deve ocorrer em três níveis:

1️⃣ **Client Runtime Validation**
- valida campos em tempo real
- evita submissões inválidas

2️⃣ **Form Submit Validation**
- valida todos os campos antes do envio

3️⃣ **Server Validation**
- backend valida novamente (regra obrigatória)

---

# Field Validation Rules

Cada campo deve possuir:

- `required`
- `type validation`
- `format validation`
- `business validation` quando necessário

Exemplo:

Email
- required
- formato email válido

CPF
- required
- formato válido
- dígito verificador válido

---

# Error Display Pattern

Erros devem seguir o padrão visual:

### Campo válido
border-gray-300
focus:ring-blue-500


### Campo inválido


border-red-500
focus:ring-red-500
bg-red-50


### Campo válido após validação


border-green-500
focus:ring-green-500
bg-green-50


---

# Tailwind Error Message Pattern

Mensagem de erro sempre abaixo do campo.

```jsx
<p className="text-sm text-red-600 mt-1">
  Mensagem de erro
</p>
Input Component Pattern

Todo input deve ser componentizado.

Exemplo:

function Input({ label, error, ...props }) {
  return (
    <div className="mb-4">

      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label}
      </label>

      <input
        {...props}
        className={`w-full rounded-lg border px-3 py-2 transition

        ${error 
            ? "border-red-500 bg-red-50 focus:ring-red-500"
            : "border-gray-300 focus:ring-blue-500"}
        `}
      />

      {error && (
        <p className="text-sm text-red-600 mt-1">
          {error}
        </p>
      )}

    </div>
  )
}
Validation Trigger Rules

Validações devem ocorrer:

OnBlur

Quando usuário sai do campo.

onBlur={validateField}
OnChange

Para campos críticos.

Exemplo:

email

cpf

telefone

Form Submit Validation

Antes de enviar formulário:

1 validar todos campos
2 bloquear envio se houver erro
3 destacar primeiro campo inválido

Exemplo:

if (!isFormValid) {
   focusFirstError()
   return
}
SweetAlert Error Pattern

Quando erro for global do formulário usar SweetAlert.

Exemplo:

erro de API

falha inesperada

validação de regra de negócio

Exemplo:

Swal.fire({
  icon: "error",
  title: "Erro",
  text: "Verifique os campos do formulário"
})
Success Pattern

Após submissão bem sucedida:

Swal.fire({
  icon: "success",
  title: "Sucesso",
  text: "Registro salvo com sucesso"
})
UX Rules

Sempre:

✔ destacar campo inválido
✔ mostrar mensagem clara
✔ evitar mensagens genéricas
✔ evitar alertas desnecessários
✔ validar antes de enviar ao backend

Nunca:

❌ mostrar erro apenas após submit
❌ permitir envio com campos inválidos
❌ usar alert() nativo do navegador

Accessibility Rules

Inputs devem possuir:

aria-invalid="true"
aria-describedby="error-id"

Exemplo:

<input
aria-invalid={error ? true : false}
aria-describedby="email-error"
/>
Componentization Rule

Formulários grandes devem ser divididos em:

Form
 ├── PersonalDataSection
 ├── AddressSection
 ├── ContactsSection
 └── DocumentsSection

Cada seção deve ser um componente React.

Folder Structure
components/
   form/
      Input.jsx
      Select.jsx
      Checkbox.jsx
      FormError.jsx

validators/
   cpfValidator.js
   emailValidator.js
   phoneValidator.js
Performance Rules

Evitar:

re-render desnecessário

validações pesadas em onChange

Preferir:

onBlur

debounce em validações complexas

Summary

Toda validação deve:

✔ ocorrer em runtime
✔ possuir feedback visual
✔ ter mensagens claras
✔ ser reutilizável
✔ seguir padrão Tailwind
✔ possuir fallback no backend


---

💡 **Sugestão importante para seu projeto:**  
O ideal é criar também **um `form_components_standard.md`**, porque isso garante que **todos os inputs, selects, masks e datepickers do sistema usem exatamente o mesmo padrão visual**.

Isso evita um dos maiores problemas em sistemas grandes: **cada tela com formulário diferente**.

Se quiser, eu também posso criar um **super padrão profissional de formulários React + Tailwind (usado em SaaS grandes)** que inclui:

- FormProvider
- Hook `useFormValidator`
- validação centralizada
- máscaras
- integração com API
- loading states
- skeleton
- auto focus
- scroll to error

Ele deixaria seu frontend **nível produto SaaS enterprise**.