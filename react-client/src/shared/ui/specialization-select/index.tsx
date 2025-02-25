import { ChangeEvent } from "react";
import { useSelector } from "react-redux";
import Loading from "../controls/Loading";
import Select from "../forms/Select";
import { RootState } from "../../../store";
import SpecializationModel from "../../api/specializations/models/specializationModel";

interface SpecializationSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  error?: string;
}

const SpecializationSelect = ({ id, name, onChange, value, disabled, className, error }: SpecializationSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchSpecializations
  );

  return (
    <div className="flex flex-col">
      {loading && <Loading label="Loading specializations..." />}
      {fetchError && <p className="text-red-500">{fetchError}</p>}
      <Select
        disabled={disabled}
        label="Specialization"
        id={id}
        name={name}
        onChange={onChange}
        value={value}
        className={className}
      >
        <option value="" label="Select specialization" />
        {data &&
          (data as SpecializationModel[]).map((spec: SpecializationModel) => (
            <option key={spec.id} value={spec.id} label={spec.specializationName} />
          ))}
      </Select>
      {error && <div className="text-red-500 mt-1">{error}</div>}
    </div>
  )
}

export default SpecializationSelect