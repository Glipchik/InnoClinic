import { RootState } from "@app/store";
import { fetchDoctorsRequest } from "@features/create-appointment-form/store/fetch-doctors";
import DoctorModel from "@models/doctors/doctorModel";
import Select from "@shared/ui/forms/Select";
import { ChangeEvent, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";

interface DoctorSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  onBlur?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  isLoadingRequired: boolean;
  specializationId?: string
  error?: string
}

const DoctorSelect = ({ id, name, onChange, value, disabled, className, error, isLoadingRequired, specializationId, onBlur }: DoctorSelectProps) => {
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
      onBlur={onBlur}
      value={value}
      className={className}
      validationError={error}
      fetchError={fetchError}
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