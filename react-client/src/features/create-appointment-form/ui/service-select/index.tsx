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
  value?: string | number;
  disabled?: boolean;
  className?: string;
  isLoadingRequired: boolean;
  specializationId?: string
  error?: string
}

const ServiceSelect = ({ id, name, onChange, value, disabled, className, isLoadingRequired, error, specializationId }: ServiceSelectProps) => {
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
      value={value}
      className={className}
      error={fetchError + '\n' + error}
      isLoading={loading}
      defaultValueLabel="Select Time Slot"
      data={data}
      mapData={(item: ServiceModel) => ({ 
        id: item.id, 
        label: item.serviceName 
      })}
    />
  )
}

export default ServiceSelect