import React from 'react';

interface FormSelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  label: string;
  error?: string;
  options: { value: string | number; label: string }[];
}

export const FormSelect: React.FC<FormSelectProps> = ({
  label,
  error,
  options,
  className = '',
  required,
  ...rest
}) => {
  return (
    <div className="mb-4 w-full">
      {label && (
        <label className="block text-sm font-medium text-gray-700 mb-1">
          {label} {required && <span className="text-red-500">*</span>}
        </label>
      )}
      
      <select
        className={`w-full rounded-md border px-3 py-2 transition-all outline-none focus:ring-2 disabled:bg-gray-100 disabled:cursor-not-allowed
          ${error ? 'border-red-500 bg-red-50 focus:ring-red-200' : 'border-gray-300 focus:ring-indigo-100 focus:border-indigo-500'}
          ${className}
        `}
        required={required}
        {...rest}
      >
        <option value="">Selecione...</option>
        {options.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>
      
      {error && <p className="text-sm text-red-600 mt-1">{error}</p>}
    </div>
  );
};
