import React from 'react';
import ReactDatePicker, { registerLocale } from 'react-datepicker';
import { ptBR } from 'date-fns/locale/pt-BR';
import { Calendar } from 'lucide-react';
import "react-datepicker/dist/react-datepicker.css";

// Register Portuguese locale
registerLocale('pt-BR', ptBR);

interface DatePickerProps {
  label: string;
  selected: Date | null | undefined;
  onChange: (date: Date | null) => void;
  error?: string;
  placeholder?: string;
  required?: boolean;
}

export const DatePicker: React.FC<DatePickerProps> = ({
  label,
  selected,
  onChange,
  error,
  placeholder = "dd/mm/aaaa",
  required
}) => {
  return (
    <div className="w-full">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {label} {required && <span className="text-red-500">*</span>}
      </label>
      <div className="relative group">
        <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 group-focus-within:text-indigo-500 transition-colors z-10 pointer-events-none">
          <Calendar size={18} />
        </div>
        <ReactDatePicker
          selected={selected}
          onChange={onChange}
          dateFormat="dd/MM/yyyy"
          locale="pt-BR"
          placeholderText={placeholder}
          isClearable
          className={`w-full pl-10 pr-3 rounded-md border py-2 h-10 transition-all outline-none focus:ring-2
            ${error
              ? "border-red-500 bg-red-50 focus:ring-red-200"
              : "border-gray-300 focus:ring-indigo-100 focus:border-indigo-500 group-hover:border-gray-400"}
          `}
        />
      </div>
      {error && (
        <p className="text-sm text-red-600 mt-1">
          {error}
        </p>
      )}

      <style>{`
        .react-datepicker-wrapper {
          width: 100%;
        }
        .react-datepicker-popper {
          z-index: 9999 !important;
        }
        .react-datepicker {
          font-family: inherit;
          border-radius: 0.75rem;
          border: 1px border-gray-100;
          box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
          overflow: hidden;
        }
        .react-datepicker__header {
          background-color: white;
          border-bottom: 1px solid #f3f4f6;
          padding-top: 1rem;
        }
        .react-datepicker__current-month {
          font-weight: 700;
          color: #111827;
          margin-bottom: 0.5rem;
        }
        .react-datepicker__day-name {
          color: #6b7280;
          font-weight: 600;
        }
        .react-datepicker__day--selected {
          background-color: #4f46e5 !important;
          border-radius: 0.5rem;
        }
        .react-datepicker__day:hover {
          border-radius: 0.5rem;
        }
        .react-datepicker__day--keyboard-selected {
          background-color: #e0e7ff;
          color: #4338ca;
          border-radius: 0.5rem;
        }
        .react-datepicker__close-icon::after {
          background-color: #9ca3af;
          font-size: 1.2rem;
        }
      `}</style>
    </div>
  );
};
