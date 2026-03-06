import React from 'react';

interface FormInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
  mask?: (value: string) => string;
}

export const FormInput: React.FC<FormInputProps> = ({ 
  label, 
  error, 
  mask, 
  onChange, 
  value,
  ...props 
}) => {
  const displayValue = mask && typeof value === 'string' ? mask(value) : value;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (onChange) {
      onChange(e);
    }
  };

  return (
    <div className="mb-4">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label}
      </label>
      <input
        {...props}
        value={displayValue || ''}
        onChange={handleChange}
        className={`w-full rounded-md border px-3 py-2 transition-all outline-none focus:ring-2
          ${error 
            ? "border-red-500 bg-red-50 focus:ring-red-200"
            : "border-gray-300 focus:ring-indigo-100 focus:border-indigo-500"}
        `}
        aria-invalid={error ? "true" : "false"}
      />
      {error && (
        <p className="text-sm text-red-600 mt-1">
          {error}
        </p>
      )}
    </div>
  );
};
