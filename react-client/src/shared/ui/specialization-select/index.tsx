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
}

const SpecializationSelect = ({id, name, onChange, value, disabled, className} : SpecializationSelectProps) => {
  const { loading, error, data } = useSelector(
    (state: RootState) => state.fetchSpecializations
  );

  return (
    <>
      {loading && <Loading label="Loading specializations..." />}
      {error && <p className="text-red-500">{error}</p>}
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
    </>
  )
}

export default SpecializationSelect