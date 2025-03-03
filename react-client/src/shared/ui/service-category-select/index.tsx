import { ChangeEvent, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import Select from "@shared/ui/forms/Select";
import { fetchServiceCategoriesRequest } from "@shared/store/fetch-service-categories";
import ServiceCategoryModel from "@models/serviceCategories/serviceCategoryModel";

interface ServiceCategorySelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  onBlur?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  isLoadingRequired: boolean;
  error?: string
}

const ServiceCategorySelect = ({ id, name, onChange, value, disabled, className, isLoadingRequired, error, onBlur }: ServiceCategorySelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchServiceCategories
  );

  const dispatch = useDispatch();
  
  useEffect(() => {
    if (isLoadingRequired) {
      dispatch(fetchServiceCategoriesRequest())
    }
  }, [isLoadingRequired, dispatch])
    
  return (
    <Select
      disabled={disabled}
      label="Service Category"
      id={id}
      name={name}
      onChange={onChange}
      onBlur={onBlur}
      value={value}
      className={className}
      validationError={error}
      fetchError={fetchError}
      isLoading={loading}
      defaultValueLabel="Select Service Category"
      data={data}
      mapData={(item: ServiceCategoryModel) => ({ 
        id: item.id, 
        label: item.categoryName 
      })}
    />
  )
}

export default ServiceCategorySelect