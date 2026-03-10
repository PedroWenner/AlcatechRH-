import React, { useState } from 'react';
import { Eye, EyeOff } from 'lucide-react';

interface FormInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
  mask?: (value: string) => string;
  icon?: React.ReactNode;
}

export const FormInput: React.FC<FormInputProps> = ({ 
  label, 
  error, 
  mask, 
  onChange, 
  value,
  icon,
  ...props 
}) => {
  const [showPassword, setShowPassword] = useState(false);
  
  const displayValue = mask && typeof value === 'string' ? mask(value) : value;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (onChange) {
      onChange(e);
    }
  };

  const currentType = props.type === 'password' && showPassword ? 'text' : props.type;

  return (
    <div className="">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label}
      </label>
      <div className="relative">
        {icon && (
          <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 pointer-events-none">
            {icon}
          </div>
        )}
        <input
          {...props}
          type={currentType}
          value={displayValue || ''}
          onChange={handleChange}
          className={`w-full rounded-md border py-2 h-10 transition-all outline-none focus:ring-2
            ${icon ? "pl-10" : "px-3"}
            ${props.type === 'password' ? "pr-10" : "pr-3"}
            ${error 
              ? "border-red-500 bg-red-50 focus:ring-red-200"
              : "border-gray-300 focus:ring-indigo-100 focus:border-indigo-500"}
          `}
          aria-invalid={error ? "true" : "false"}
        />
        {props.type === 'password' && (
          <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
            className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600 focus:outline-none"
          >
            {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
          </button>
        )}
      </div>
      {error && (
        <p className="text-sm text-red-600 mt-1">
          {error}
        </p>
      )}
    </div>
  );
};
