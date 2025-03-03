import React, { ChangeEvent } from 'react';
import Label from '../containers/Label';

interface InputProps {
  value: string;
  label?: string;
  name: string;
  placeholder?: string;
  min?: string,
  max?: string,
  step?: string,
  type: string;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onBlur?: (e: ChangeEvent<HTMLInputElement>) => void;
  id?: string;
  error?: string
}

const Input: React.FC<InputProps> = ({ value, label, name, placeholder, type, onChange, id, error, onBlur, min, max, step }) => (
  <div className="form-group flex flex-col">
    {label && <label htmlFor="input-field" className="text-gray-700 font-medium">{label}</label>}
    <input
      id={id}
      type={type}
      value={value}
      name={name}
      min={min}
      max={max}
      step={step}
      className="form-control mt-1 p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
      placeholder={placeholder}
      onChange={onChange}
      onBlur={onBlur}
    />
    {error && <Label type="error" value={error} />}
  </div>
);

export default Input;