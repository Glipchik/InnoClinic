import React, { ReactNode, ChangeEvent } from 'react';

interface SelectProps {
  label?: string;
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  children: ReactNode;
  disabled?: boolean;
  className?: string;
}

const Select: React.FC<SelectProps> = ({ label, id, name, onChange, value, children, disabled, className }) => (
  <div className="form-group flex flex-col">
    {label && <label htmlFor={id} className="font-medium">{label}</label>}
    <select
      id={id}
      value={value}
      name={name}
      className={`mt-1 p-2 text-black border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${className || ''}`}
      onChange={onChange}
      disabled={disabled}
    >
      {children}
    </select>
  </div>
);

export default Select;