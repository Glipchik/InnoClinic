import { ChangeEvent, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import Select from "@shared/ui/forms/Select";
import TimeSlot from "@entities/timeSlot";
import { fetchDoctorScheduleRequest } from "@features/create-appointment-form/store/fetch-doctor-schedule";
import '@shared/lib/dateStringExtension'

interface TimeSlotSelectProps {
  id?: string;
  name?: string;
  onChange?: (event: ChangeEvent<HTMLSelectElement>) => void;
  onBlur?: (event: ChangeEvent<HTMLSelectElement>) => void;
  value?: string | number;
  disabled?: boolean;
  className?: string;
  isLoadingRequired: boolean;
  date?: string,
  doctorId?: string
  error?: string
}

const TimeSlotSelect = ({ id, name, onChange, value, disabled, className, isLoadingRequired, error, date, doctorId, onBlur }: TimeSlotSelectProps) => {
  const { loading, error: fetchError, data } = useSelector(
    (state: RootState) => state.fetchDoctorSchedule
  );

  const dispatch = useDispatch();

  useEffect(() => {
    if (isLoadingRequired && date && doctorId) {
      dispatch(fetchDoctorScheduleRequest({ date: new Date(date).toISODateString(), doctorId: doctorId }))
    }
  }, [isLoadingRequired])

  return (
    <Select
      disabled={disabled}
      label="Available Time Slots"
      id={id}
      name={name}
      onChange={onChange}
      onBlur={onBlur}
      value={value}
      className={className}
      validationError={error}
      fetchError={fetchError}
      isLoading={loading}
      defaultValueLabel="Select Time Slot"
      data={data}
      mapData={(item: TimeSlot) => ({
        id: item.id.toString(),
        label: item.start
      })}
    />
  )
}

export default TimeSlotSelect