import React, { ChangeEvent } from "react";
import Label from "../containers/Label";

interface CheckboxProps {
  checked: boolean;
  label?: string;
  name: string;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onBlur: (e: ChangeEvent<HTMLInputElement>) => void;
  id?: string;
  error?: string;
  data_testid?: string;
}

const Checkbox: React.FC<CheckboxProps> = ({
  checked,
  label,
  name,
  onChange,
  onBlur,
  id,
  error,
  data_testid,
}) => (
  <div className="form-group flex flex-col gap-y-2" data-testid={data_testid}>
    <label className="flex items-center gap-2">
      <input
        id={id}
        type="checkbox"
        name={name}
        checked={checked}
        onChange={onChange}
        onBlur={onBlur}
        className="form-checkbox h-5 w-5 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
      />
      {label && <span className="text-gray-700 font-medium">{label}</span>}
    </label>
    {error && <Label type="error" value={error} />}
  </div>
);

export default Checkbox;
