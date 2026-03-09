# Page Header Standard

All CRUD view pages must implement the following unified Page Header block. It provides a consistent title, semantic icon from `lucide-react`, descriptive subtitle, and the primary action button.

```tsx
<div className="flex justify-between items-center bg-white p-6 rounded-lg shadow-sm border border-gray-200 mb-6">
  <div>
    <h1 className="text-2xl font-bold text-gray-800 flex items-center gap-2">
      <Briefcase className="text-indigo-600" /> {/* Use an appropriate semantic icon */}
      Gestão de Cargos
    </h1>
    <p className="text-gray-500 mt-1">Gerencie os cargos e CBOs da empresa.</p>
  </div>
  {hasPermission('Cargos', 'Criar') && (
    <button
      onClick={openNewModal}
      className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors shadow-sm"
    >
      <Plus size={20} className="mr-2" />
      Novo Cadastro
    </button>
  )}
</div>
```
