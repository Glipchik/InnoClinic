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
}

const TimeSlotSelect = ({id, name, onChange, value, disabled, className} : TimeSlotSelectProps) => {
  const { loading, error, data } = useSelector(
    (state: RootState) => state.fetchDoctorSchedule
  );

  return (
    <>
      {loading && <Loading label="Loading Time Slots..." />}
      {error && <p className="text-red-500">{error}</p>}
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
    </>
  )
}

export default TimeSlotSelect