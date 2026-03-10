# Modals Standards

## Purpose

Padronizar a construção e exibição de janelas modais (`<Modal>`) por toda a aplicação. 
Modais são utilizados primariamente para Criação e Edição de registros (CRUDs) ou painéis de configuração que não exigem uma mudança massiva de contexto de página.

---

## 🛑 Regra Crítica: Footer Padrão (Action Buttons)

**Nunca** crie botões de ação (Salvar, Cancelar, etc.) internamente fixados no corpo final do `<form>` ou na tag principal do modal. 
O componente `<Modal>` do sistema possui uma prop nativa chamada `footer` desenhada especificamente para fixar essas ações em seu rodapé respeitando margens e semântica de interface.

### ❌ Incorreto (Botões dentro do corpo do modal/form)
```tsx
<Modal isOpen={isOpen} onClose={close} title="Editar">
  <form onSubmit={handleSubmit}>
    <FormInput label="Nome" />
    
    {/* Errado: Botões na base estrutural do conteúdo interno */}
    <div className="flex justify-end space-x-3 mt-6">
      <button type="button" onClick={close}>Cancelar</button>
      <button type="submit">Salvar</button>
    </div>
  </form>
</Modal>
```

### ✅ Correto (Botões injetados via prop `footer` no Modal)
```tsx
<Modal 
  isOpen={isOpen} 
  onClose={close} 
  title="Editar"
  footer={(
    <>
      <button
        type="submit"
        form="my-custom-form-id" // Linka o botão com o form interno
        className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
      >
        Salvar
      </button>
      <button
        type="button"
        onClick={close}
        className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
      >
        Cancelar
      </button>
    </>
  )}
>
  <form id="my-custom-form-id" onSubmit={handleSubmit} className="space-y-4">
    <FormInput label="Nome" />
  </form>
</Modal>
```

> **Atenção:** Como os botões agora ficam fisicamente isolados da árvore DOM do `<form>`, certifique-se de referenciar o evento `submit` passando no botão principal `type="submit"` a propriedade `form="id-do-formulario"` (ou lide via evento `onClick` externo de envio no pai).
