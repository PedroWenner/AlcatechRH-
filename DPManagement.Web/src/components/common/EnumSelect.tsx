import { useCatalogos } from '../../hooks/useCatalogos';
import type { CatalogoItem } from '../../hooks/useCatalogos';

interface EnumSelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  label: string;
  enumType: 'RegimeJuridico' | 'FormaIngresso';
  error?: string;
}

export function EnumSelect({ label, enumType, error, className = '', ...rest }: EnumSelectProps) {
  const { regimesJuridicos, formasIngresso, isLoading, hasError } = useCatalogos();

  let options: CatalogoItem[] = [];
  if (enumType === 'RegimeJuridico') options = regimesJuridicos;
  if (enumType === 'FormaIngresso') options = formasIngresso;

  return (
    <div className="mb-4 w-full">
      {label && <label className="block text-sm font-medium text-gray-700 mb-1">{label} {rest.required && <span className="text-red-500">*</span>}</label>}
      
      <select
        className={`w-full rounded-md border px-3 py-2 transition-all outline-none focus:ring-2 disabled:bg-gray-100 disabled:cursor-not-allowed
          ${error ? 'border-red-500 bg-red-50 focus:ring-red-200' : 'border-gray-300 focus:ring-indigo-100 focus:border-indigo-500'}
          ${className}
        `}
        disabled={isLoading || hasError || rest.disabled}
        {...rest}
      >
        <option value="">
          {isLoading ? 'Carregando opções...' : hasError ? 'Erro ao carregar' : 'Selecione...'}
        </option>
        
        {!isLoading && !hasError && options.map(opt => (
          <option key={opt.id} value={opt.id}>{opt.nome}</option>
        ))}
      </select>
      
      {error && <p className="text-sm text-red-600 mt-1">{error}</p>}
    </div>
  );
}
