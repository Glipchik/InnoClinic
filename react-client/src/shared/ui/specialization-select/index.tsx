import { ChangeEvent, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import Select from "../forms/Select";
import SpecializationModel from "../../models/specializations/specializationModel";
import { RootState } from "@app/store";
import { fetchSpecializationsRequest } from "@shared/store/fetch-specializations";

interface SpecializationSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  onBlur?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  isLoadingRequired: boolean
  error?: string
}

const SpecializationSelect = ({ id, name, onChange, onBlur, value, disabled, className, isLoadingRequired, error }: SpecializationSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchSpecializations
  );

  const dispatch = useDispatch();

  useEffect(() => {
    if (isLoadingRequired) {
      dispatch(fetchSpecializationsRequest())
    }
  }, [isLoadingRequired])

  return (
    <Select
      disabled={disabled}
      label="Specialization"
      id={id}
      name={name}
      onChange={onChange}
      onBlur={onBlur}
      value={value}
      className={className}
      data={data}
      defaultValueLabel="Select specialization"
      validationError={error}
      fetchError={fetchError}
      isLoading={loading}
      mapData={(item: SpecializationModel) => ({ 
        id: item.id, 
        label: item.specializationName 
      })}
    />
  )
}

export default SpecializationSelect