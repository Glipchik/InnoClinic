import { ChangeEvent } from "react";
import { useSelector } from "react-redux";
import Loading from "../controls/Loading";
import Select from "../forms/Select";
import { RootState } from "../../../store";
import ServiceModel from "../../api/services/models/serviceModel";

interface ServiceSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  error?: string;
}

const ServiceSelect = ({id, name, onChange, value, disabled, className, error} : ServiceSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchServices
  );

  return (
    <div className="flex flex-col">
      {loading && <Loading label="Loading services..." />}
      {fetchError && <p className="text-red-500">{fetchError}</p>}
      <Select
        disabled={disabled}
        label="Service"
        id={id}
        name={name}
        onChange={onChange}
        value={value}
        className={className}
      >
        <option value="" label="Select service" />
        {data &&
          (data as ServiceModel[]).map((service: ServiceModel) => (
            <option key={service.id} value={service.id} label={service.serviceName} />
          ))}
      </Select>
      {error && <div className="text-red-500 mt-1">{error}</div>}
    </div>
  )
}

export default ServiceSelect