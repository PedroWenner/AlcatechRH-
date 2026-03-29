import { useState, useCallback } from 'react';

export interface ValidationRules {
  [field: string]: {
    required?: string;
    minLength?: { value: number; message: string };
    maxLength?: { value: number; message: string };
    pattern?: { value: RegExp; message: string };
    custom?: (value: any) => string | undefined;
  };
}

export interface UseFormValidationOptions {
  rules: ValidationRules;
  initialValues?: Record<string, any>;
}

export function useFormValidation({ rules }: UseFormValidationOptions) {
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [showErrors, setShowErrors] = useState(false);

  const validateField = useCallback((field: string, value: any): string => {
    const rule = rules[field];
    if (!rule) return '';

    if (rule.required && (!value || (typeof value === 'string' && !value.trim()))) {
      return rule.required;
    }

    if (rule.minLength && typeof value === 'string' && value.length < rule.minLength.value) {
      return rule.minLength.message;
    }

    if (rule.maxLength && typeof value === 'string' && value.length > rule.maxLength.value) {
      return rule.maxLength.message;
    }

    if (rule.pattern && typeof value === 'string' && !rule.pattern.value.test(value)) {
      return rule.pattern.message;
    }

    if (rule.custom) {
      return rule.custom(value) || '';
    }

    return '';
  }, [rules]);

  const validateAll = useCallback((values: Record<string, any>): boolean => {
    const newErrors: Record<string, string> = {};

    Object.keys(rules).forEach(field => {
      const error = validateField(field, values[field]);
      if (error) {
        newErrors[field] = error;
      }
    });

    setErrors(newErrors);
    setShowErrors(true);
    return Object.keys(newErrors).length === 0;
  }, [rules, validateField]);

  const setFieldError = useCallback((field: string, error: string) => {
    setErrors(prev => ({ ...prev, [field]: error }));
  }, []);

  const clearFieldError = useCallback((field: string) => {
    setErrors(prev => {
      const newErrors = { ...prev };
      delete newErrors[field];
      return newErrors;
    });
  }, []);

  const clearAllErrors = useCallback(() => {
    setErrors({});
    setShowErrors(false);
  }, []);

  const getError = useCallback((field: string): string | undefined => {
    return showErrors ? errors[field] : undefined;
  }, [errors, showErrors]);

  return {
    errors,
    showErrors,
    setShowErrors,
    validateField,
    validateAll,
    setFieldError,
    clearFieldError,
    clearAllErrors,
    getError,
  };
}
