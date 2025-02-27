import { ChangeEvent, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import Select from "@shared/ui/forms/Select";
import ServiceModel from "@models/services/serviceModel";
import { fetchServicesRequest } from "@shared/store/slices/fetch-services";

interface ServiceSelectProps {
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

const ServiceSelect = ({ id, name, onChange, value, disabled, className, isLoadingRequired, error, specializationId, onBlur }: ServiceSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchServices
  );

  const dispatch = useDispatch();
  
  useEffect(() => {
    if (isLoadingRequired) {
      if (specializationId)
        dispatch(fetchServicesRequest({specializationId}))
    }
  }, [isLoadingRequired, dispatch])
    
  return (
    <Select
      disabled={disabled}
      label="Service"
      id={id}
      name={name}
      onChange={onChange}
      onBlur={onBlur}
      value={value}
      className={className}
      validationError={error}
      fetchError={fetchError}
      isLoading={loading}
      defaultValueLabel="Select Service"
      data={data}
      mapData={(item: ServiceModel) => ({ 
        id: item.id, 
        label: item.serviceName 
      })}
    />
  )
}

export default ServiceSelect