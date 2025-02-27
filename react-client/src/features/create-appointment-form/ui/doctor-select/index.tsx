import { RootState } from "@app/store";
import DoctorModel from "@models/doctors/doctorModel";
import { fetchDoctorsRequest } from "@shared/store/slices/fetch-doctors";
import Select from "@shared/ui/forms/Select";
import { ChangeEvent, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";

interface DoctorSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  isLoadingRequired: boolean;
  specializationId?: string
  error?: string
}

const DoctorSelect = ({ id, name, onChange, value, disabled, className, error, isLoadingRequired, specializationId }: DoctorSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchDoctors
  );

  const dispatch = useDispatch();
    
  useEffect(() => {
    if (isLoadingRequired) {
      if (specializationId)
        dispatch(fetchDoctorsRequest({specializationId}))
    }
  }, [isLoadingRequired, dispatch])

  return (
    <Select
      disabled={disabled}
      label="Doctor"
      id={id}
      name={name}
      onChange={onChange}
      value={value}
      className={className}
      error={fetchError + '\n' + error}
      isLoading={loading}
      defaultValueLabel="Select Doctor"
      data={data}
      mapData={(item: DoctorModel) => ({ 
        id: item.id, 
        label: item.firstName + ' ' + item.lastName 
      })}
    />
  )
}

export default DoctorSelect