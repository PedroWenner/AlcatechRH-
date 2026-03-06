import React, { useState, useEffect, useRef } from 'react';
import { Search, Loader2, X } from 'lucide-react';

interface AutocompleteProps {
  label: string;
  placeholder?: string;
  onSearch: (term: string) => Promise<any[]>;
  onSelect: (item: any) => void;
  defaultValue?: string;
  displayValue: (item: any) => string;
  keyValue: (item: any) => string;
  required?: boolean;
  error?: string;
}

export function Autocomplete({
  label,
  placeholder = 'Digite para buscar...',
  onSearch,
  onSelect,
  defaultValue = '',
  displayValue,
  keyValue,
  required = false,
  error
}: AutocompleteProps) {
  const [searchTerm, setSearchTerm] = useState(defaultValue);
  const [results, setResults] = useState<any[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const [selectedIndex, setSelectedIndex] = useState(-1);
  const wrapperRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    setSearchTerm(defaultValue);
  }, [defaultValue]);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (wrapperRef.current && !wrapperRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    }
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  useEffect(() => {
    const delayDebounceFn = setTimeout(async () => {
      if (searchTerm.length >= 2 && isOpen) {
        setIsLoading(true);
        try {
          const data = await onSearch(searchTerm);
          setResults(data);
          setSelectedIndex(-1);
        } catch (error) {
          console.error('Erro na busca autocomplete:', error);
        } finally {
          setIsLoading(false);
        }
      } else {
        setResults([]);
      }
    }, 300);

    return () => clearTimeout(delayDebounceFn);
  }, [searchTerm, onSearch, isOpen]);

  const handleSelect = (item: any) => {
    onSelect(item);
    setSearchTerm(displayValue(item));
    setIsOpen(false);
    setResults([]);
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'ArrowDown') {
      setSelectedIndex(prev => (prev < results.length - 1 ? prev + 1 : prev));
    } else if (e.key === 'ArrowUp') {
      setSelectedIndex(prev => (prev > 0 ? prev - 1 : prev));
    } else if (e.key === 'Enter' && selectedIndex >= 0) {
      e.preventDefault();
      handleSelect(results[selectedIndex]);
    } else if (e.key === 'Escape') {
      setIsOpen(false);
    }
  };

  return (
    <div className="relative mb-4" ref={wrapperRef}>
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label} {required && <span className="text-red-500">*</span>}
      </label>
      <div className="relative">
        <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
          <Search size={16} className="text-gray-400" />
        </div>
        <input
          type="text"
          className={`w-full bg-white border ${error ? 'border-red-500' : 'border-gray-300'} rounded-md py-2 pl-10 pr-10 text-sm focus:outline-none focus:ring-2 ${error ? 'focus:ring-red-100' : 'focus:ring-indigo-100 focus:border-indigo-500'} transition-all`}
          placeholder={placeholder}
          value={searchTerm}
          onChange={(e) => {
            setSearchTerm(e.target.value);
            setIsOpen(true);
          }}
          onFocus={() => setIsOpen(true)}
          onKeyDown={handleKeyDown}
        />
        <div className="absolute inset-y-0 right-0 pr-3 flex items-center">
          {isLoading ? (
            <Loader2 size={16} className="text-indigo-500 animate-spin" />
          ) : searchTerm && (
            <button 
              type="button"
              onClick={() => { setSearchTerm(''); onSelect(null); }}
              className="text-gray-400 hover:text-gray-600"
            >
              <X size={16} />
            </button>
          )}
        </div>
      </div>

      {isOpen && (results.length > 0 || (searchTerm.length >= 2 && !isLoading)) && (
        <div className="absolute z-[1002] w-full mt-1 bg-white shadow-xl max-h-60 rounded-md py-1 text-base ring-1 ring-black ring-opacity-5 overflow-auto focus:outline-none sm:text-sm border border-gray-100 animate-in fade-in slide-in-from-top-2 duration-200">
          {results.length > 0 ? (
            results.map((item, index) => (
              <div
                key={keyValue(item)}
                className={`cursor-pointer select-none relative py-2 pl-3 pr-9 transition-colors ${selectedIndex === index ? 'bg-indigo-600 text-white' : 'text-gray-900 hover:bg-indigo-50 hover:text-indigo-900'}`}
                onClick={() => handleSelect(item)}
              >
                <div className="flex flex-col">
                  <span className={`block truncate ${selectedIndex === index ? 'font-semibold' : 'font-normal'}`}>
                    {displayValue(item)}
                  </span>
                  <span className={`block truncate text-xs ${selectedIndex === index ? 'text-indigo-100' : 'text-gray-500'}`}>
                    Código: {keyValue(item)}
                  </span>
                </div>
              </div>
            ))
          ) : searchTerm.length >= 2 && (
            <div className="py-2 pl-3 pr-9 text-gray-500 italic text-sm">
              Nenhum resultado encontrado.
            </div>
          )}
        </div>
      )}
      {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
    </div>
  );
}
