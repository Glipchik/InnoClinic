import React, { ChangeEvent } from 'react';

interface InputProps {
  value: string;
  label?: string;
  name: string;
  placeholder?: string;
  type: string;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  id?: string;
}

const Input: React.FC<InputProps> = ({ value, label, name, placeholder, type, onChange, id }) => (
  <div className="form-group flex flex-col">
    {label && <label htmlFor="input-field" className="text-gray-700 font-medium">{label}</label>}
    <input
      id={id}
      type={type}
      value={value}
      name={name}
      className="form-control mt-1 p-2 border text-black border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
      placeholder={placeholder}
      onChange={onChange}
    />
  </div>
);

export default Input;