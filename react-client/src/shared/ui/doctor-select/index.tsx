import { ChangeEvent } from "react";
import { useSelector } from "react-redux";
import Loading from "../controls/Loading";
import Select from "../forms/Select";
import { RootState } from "../../../store";
import DoctorModel from "../../api/doctors/models/doctorModel";

interface DoctorSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  error?: string;
}

const DoctorSelect = ({id, name, onChange, value, disabled, className, error} : DoctorSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchDoctors
  );

  return (
    <div className="flex flex-col">
      {loading && <Loading label="Loading doctors..." />}
      {fetchError && <p className="text-red-500">{fetchError}</p>}
      <Select
        disabled={disabled}
        label="Doctor"
        id={id}
        name={name}
        onChange={onChange}
        value={value}
        className={className}
      >
        <option value="" label="Select doctor" />
        {data &&
          (data as DoctorModel[]).map((doc: DoctorModel) => (
            <option key={doc.id} value={doc.id} label={doc.firstName + ' ' + doc.lastName} />
          ))}
      </Select>
      {error && <div className="text-red-500 mt-1">{error}</div>}
    </div>
  )
}

export default DoctorSelect