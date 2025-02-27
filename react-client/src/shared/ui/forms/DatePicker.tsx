import React, { ChangeEvent } from 'react';
import Label from '../containers/Label';

interface DatePickerProps {
  label?: string;
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLInputElement>) => void;
  onBlur?: (event: ChangeEvent<HTMLInputElement>) => void;
  value?: Date | string;
  disabled?: boolean;
  className?: string;
  error?: string;
}

const formatDate = (date?: Date | string): string => {
  if (!date) return "";
  if (typeof date === "string") return date;

  return date.toISOString().split("T")[0];
};

const DatePicker: React.FC<DatePickerProps> = ({ label, id, name, onChange, onBlur, value, disabled, className, error }) => (
  <div className="form-group flex flex-col">
    {label && <label htmlFor={id} className="text-gray-700 font-medium">{label}</label>}
    <input
      type="date"
      id={id}
      name={name}
      value={formatDate(value)}
      onBlur={onBlur}
      onChange={onChange}
      disabled={disabled}
      className={`mt-1 p-2 text-black border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${className || ''}`}
    />
    {error && <Label type="error" value={error} />}
  </div>
);

export default DatePicker;
