# Datepicker Standards

## Purpose

Padronizar o uso de campos de data no sistema utilizando **React + TailwindCSS**, garantindo:

- consistência visual
- experiência previsível para o usuário
- formato de data padronizado
- validação correta antes do envio para API

Todos os campos de data devem seguir este padrão.

---

# Default Date Format

O formato padrão do sistema deve ser:


DD/MM/YYYY


Exemplo:


25/03/2026


Para **data e hora**:


DD/MM/YYYY HH:mm


Exemplo:


25/03/2026 14:30


---

# Datepicker Library

Biblioteca recomendada:


react-datepicker


ou


flatpickr


Caso o projeto utilize TailAdmin, priorizar integração visual com Tailwind.

---

# Import Pattern

Exemplo usando react-datepicker:

```typescript
import DatePicker from "react-datepicker"
import "react-datepicker/dist/react-datepicker.css"
Visual Style Pattern

Todo datepicker deve seguir o padrão visual de inputs do sistema.

Classe padrão:

className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm focus:border-blue-500 focus:ring-blue-500"

Estado inválido:

className="border-red-500 bg-red-50 focus:ring-red-500"

Estado válido:

className="border-green-500 bg-green-50 focus:ring-green-500"
Datepicker Component Pattern

Todos os campos de data devem ser componentizados.

Arquivo sugerido:

components/form/DatePickerInput.tsx

Exemplo:

import DatePicker from "react-datepicker"

interface Props {
  label: string
  value: Date | null
  onChange: (date: Date | null) => void
  error?: string
}

export default function DatePickerInput({
  label,
  value,
  onChange,
  error
}: Props) {

  return (

    <div className="mb-4">

      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label}
      </label>

      <DatePicker
        selected={value}
        onChange={onChange}
        dateFormat="dd/MM/yyyy"
        className={`w-full rounded-lg border px-3 py-2 text-sm
        ${error ? "border-red-500 bg-red-50" : "border-gray-300"}`}
      />

      {error && (
        <p className="text-red-600 text-sm mt-1">
          {error}
        </p>
      )}

    </div>

  )
}
Input Mask Pattern

O campo de data deve permitir digitação manual.

Máscara recomendada:

99/99/9999

Exemplo válido:

31/12/2026
Validation Rules

Todos os campos de data devem validar:

1 formato válido
2 data existente
3 regra de negócio quando necessário

Exemplos:

data de nascimento não pode ser futura

data de admissão não pode ser maior que data atual

data final não pode ser menor que data inicial

Min and Max Date

Quando aplicável, definir limites.

Exemplo:

<DatePicker
selected={date}
onChange={setDate}
minDate={new Date(1900, 0, 1)}
maxDate={new Date()}
/>
Date Range Pattern

Quando selecionar período:

Exemplo:

Data inicial
Data final

Regra:

dataFinal >= dataInicial

Exemplo:

<DatePicker
selected={startDate}
onChange={setStartDate}
/>

<DatePicker
selected={endDate}
onChange={setEndDate}
minDate={startDate}
/>
Default UX Rules

Datepicker deve:

✔ abrir calendário ao clicar no campo
✔ permitir digitação manual
✔ permitir navegação por mês/ano
✔ fechar automaticamente ao selecionar data

Evitar:

❌ exigir múltiplos cliques para selecionar data
❌ bloquear digitação manual
❌ aceitar formato inválido

Accessibility

Campos devem possuir:

aria-label
aria-invalid

Exemplo:

<input
aria-label="Data de nascimento"
aria-invalid={error ? true : false}
/>
Backend Integration

Datas enviadas para API devem estar no formato:

YYYY-MM-DD

Exemplo:

2026-03-25

Conversão recomendada:

date.toISOString().split("T")[0]
Common Date Fields

Campos comuns no sistema:

dataNascimento

dataAdmissao

dataDemissao

dataInicio

dataFim

dataPagamento

dataCompetencia

Todos devem utilizar o mesmo componente DatePickerInput.

Folder Structure
components/
   form/
      DatePickerInput.tsx

utils/
   dateFormatter.ts
   dateValidator.ts
Summary

Todos os campos de data devem:

✔ usar componente padrão
✔ utilizar formato DD/MM/YYYY
✔ validar datas inválidas
✔ possuir máscara
✔ manter consistência visual com Tailwind


---

💡 **Recomendação MUITO importante para seu projeto de DP / eSocial**