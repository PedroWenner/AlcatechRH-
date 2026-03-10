import React, { useState } from 'react';
import { Search, X, Filter } from 'lucide-react';

interface FilterBarProps {
  onFilter: (filters: any) => void;
  onClear: () => void;
  children: React.ReactNode;
}

export function FilterBar({ onFilter, onClear, children }: FilterBarProps) {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className="bg-white p-4 rounded-t-lg border-b border-gray-100 shadow-sm mb-1">
      <div className="flex justify-between items-center">
        <div className="flex items-center text-gray-700 font-medium">
          <Filter size={18} className="mr-2 text-indigo-600" />
          Filtros de Busca
        </div>
        <div className="flex space-x-2">
          <button
            onClick={() => setIsOpen(!isOpen)}
            className="text-sm text-indigo-600 hover:text-indigo-800 font-medium transition-colors"
          >
            {isOpen ? 'Ocultar Filtros' : 'Mostrar Filtros'}
          </button>
        </div>
      </div>

      {isOpen && (
        <div className="mt-4 pt-4 border-t border-gray-50">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {children}
          </div>
          <div className="mt-4 flex justify-end space-x-3">
            <button
              type="button"
              onClick={onClear}
              className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 flex items-center transition-all"
            >
              <X size={16} className="mr-2" />
              Limpar
            </button>
            <button
              type="button"
              onClick={onFilter}
              className="px-6 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 flex items-center shadow-sm transition-all active:scale-95"
            >
              <Search size={16} className="mr-2" />
              Filtrar
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
