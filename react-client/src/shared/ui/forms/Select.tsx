import React, { ChangeEvent } from 'react';
import Label from '../containers/Label';
import Loading from '../controls/Loading';

interface SelectProps {
  label?: string;
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  onBlur?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  validationError?: string,
  fetchError?: string
  isLoading?: boolean;
  data?: unknown[];
  defaultValueLabel?: string;
  mapData: (item: never) => { id: string, label: string };
}

const Select: React.FC<SelectProps> = ({ 
    label, 
    id, 
    name, 
    onChange, 
    onBlur,
    value,
    disabled, 
    className,
    fetchError,
    validationError,
    isLoading,
    data,
    defaultValueLabel,
    mapData
  }: SelectProps) => (
  <div className="form-group flex flex-col">
    {label && <label htmlFor={id} className="font-medium">{label}</label>}
    {isLoading && <Loading label="Loading..." />}
    {validationError && <Label type="error" value={validationError} />}
    {fetchError && <Label type="error" value={fetchError} />}
    <select
      id={id}
      value={value}
      name={name}
      className={`mt-1 p-2 text-black border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${className || ''}`}
      onChange={onChange}
      onBlur={onBlur}
      disabled={disabled}
    >
      {defaultValueLabel && <option value="" label={defaultValueLabel} />}
      {data && mapData &&
        data.map((item) => {
          const { id, label } = mapData(item);
          return <option key={id} value={id} label={label} />;
        })
      }
    </select>
  </div>
);

export default Select;