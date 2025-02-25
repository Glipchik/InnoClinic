import { ChangeEvent } from "react";
import { useSelector } from "react-redux";
import Loading from "../controls/Loading";
import Select from "../forms/Select";
import { RootState } from "../../../store";
import TimeSlot from "../../../entities/timeSlot";

interface TimeSlotSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  error?: string;
}

const TimeSlotSelect = ({id, name, onChange, value, disabled, className, error} : TimeSlotSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchDoctorSchedule
  );

  return (
    <div className="flex flex-col">
      {loading && <Loading label="Loading Time Slots..." />}
      {fetchError && <p className="text-red-500">{fetchError}</p>}
      <Select
        disabled={disabled}
        label="Available Time Slots"
        id={id}
        name={name}
        onChange={onChange}
        value={value}
        className={className}
      >
        <option value="" label="Select Time Slot" />
        {data &&
          (data as TimeSlot[]).map((ts: TimeSlot) => (
            <option key={ts.id} value={ts.id} label={ts.start} />
          ))}
      </Select>
      {error && <div className="text-red-500 mt-1">{error}</div>}
    </div>
  )
}

export default TimeSlotSelect