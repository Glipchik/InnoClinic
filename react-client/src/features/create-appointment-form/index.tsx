import type React from "react"
import { useState, useEffect } from "react"
import { validationSchema } from "./models/validationSchema"
import { MIN_APPOINTMENT_DATE } from "./lib/dateUtils"
import DatePicker from "../../shared/ui/forms/DatePicker"
import { useFormik } from 'formik'
import { useDispatch, useSelector } from "react-redux"
import { fetchSpecializationsRequest } from "../../shared/store/specializations"
import { fetchServicesRequest } from "../../shared/store/services"
import { fetchDoctorsRequest } from "../../shared/store/doctors"
import { fetchDoctorScheduleRequest } from "../../shared/store/doctor-schedule"
import SpecializationSelect from "../../shared/ui/specialization-select"
import ServiceSelect from "../../shared/ui/service-select"
import DoctorSelect from "../../shared/ui/doctor-select"
import TimeSlotSelect from "../../shared/ui/time-slot-select"
import CreateAppointmentModel from "./models/createAppointmentModel"
import { createAppointmentRequest } from "./models/store"
import { RootState } from "../../store"
import Loading from "../../shared/ui/controls/Loading"
import ErrorBox from "../../shared/ui/containers/ErrorBox"
import CancelAndSubmit from "../../shared/ui/widgets/cancel-and-submit"

interface CreateAppointmentFormProps {
  onCancel: () => void
}

const CreateAppointmentForm = ({onCancel} : CreateAppointmentFormProps) => {
  const dispatch = useDispatch();
  const [date, setDate] = useState<string | null>(null)
  const { loading, error } = useSelector(
    (state: RootState) => state.createAppointment
  );

  useEffect(() => {
    dispatch(fetchSpecializationsRequest())
  }, [dispatch])

  const formik = useFormik({
    initialValues: {
      serviceId: "",
      specializationId: "",
      date: MIN_APPOINTMENT_DATE.toISOString().split("T")[0],
      timeSlotId: -1,
      doctorId: "",
    },
    validationSchema,
    onSubmit: (values) => onSubmit(values as CreateAppointmentModel),
  })

  const handleSpecializationChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const specializationId = e.target.value
    formik.setFieldValue("specializationId", specializationId)

    if (specializationId) {
      dispatch(fetchServicesRequest(specializationId))
      dispatch(fetchDoctorsRequest(specializationId))
    }
  }

  const handleDoctorChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const doctorId = e.target.value
    formik.setFieldValue("doctorId", doctorId)

    if (doctorId && date) {
      dispatch(fetchDoctorScheduleRequest({ doctorId, date: new Date(date) }))
    }
  }

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const selectedDate = e.target.value;
    setDate(selectedDate);
    formik.setFieldValue("date", selectedDate);

    if (formik.values.doctorId && selectedDate) {
      dispatch(fetchDoctorScheduleRequest({ doctorId: formik.values.doctorId, date: new Date(selectedDate)}))
    }
  }

  const onSubmit = (values: CreateAppointmentModel) => {
    dispatch(createAppointmentRequest(values))
  }

  return (
    <form onSubmit={formik.handleSubmit} className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <div className="flex flex-col">
        <SpecializationSelect
          disabled={false}
          id="specialization-select-for-create-appointment-form-id"
          name="specializationId"
          value={formik.values.specializationId}
          onChange={handleSpecializationChange}
        />
        {formik.touched.specializationId && formik.errors.specializationId && (
          <div className="text-red-500 mt-1">{formik.errors.specializationId}</div>
        )}
      </div>

      <div className="flex flex-col">
        <ServiceSelect
          id="service-select-for-create-appointment-form-id"
          disabled={formik.values.specializationId ? false : true}
          name="serviceId"
          onChange={formik.handleChange}
          value={formik.values.serviceId}
          className={formik.values.specializationId ? "" : "opacity-50 cursor-not-allowed"}
        />
        {formik.touched.serviceId && formik.errors.serviceId && (
          <div className="text-red-500 mt-1">{formik.errors.serviceId}</div>
        )}
      </div>

      <div className="flex flex-col">
        <DoctorSelect 
          id="doctor-select-for-create-appointment-form-id"
          disabled={formik.values.specializationId ? false : true}
          name="doctorId"
          onChange={handleDoctorChange}
          value={formik.values.doctorId}
          className={formik.values.specializationId ? "" : "opacity-50 cursor-not-allowed"}
        />
        {formik.touched.doctorId && formik.errors.doctorId && (
          <div className="text-red-500 mt-1">{formik.errors.doctorId}</div>
        )}
      </div>

      <div className="flex flex-col">
        <DatePicker
          label="Choose a date for the appointment"
          id="select-date-for-create-appointment-form-id"
          name="date"
          value={formik.values.date}
          onChange={handleDateChange}
          disabled={false}
        />
        {formik.touched.date && formik.errors.date && (
          <div className="text-red-500 mt-1">{formik.errors.date}</div>
        )}
      </div>

      <div className="flex flex-col">
        <TimeSlotSelect
          id="time-slot-select-for-create-appointment-form-id"
          disabled={(formik.values.date && formik.values.doctorId) ? false : true}
          name="timeSlotId"
          onChange={formik.handleChange}
          value={formik.values.timeSlotId}
          className={(formik.values.date && formik.values.doctorId) ? "" : "opacity-50 cursor-not-allowed"}
        />
        {formik.touched.timeSlotId && formik.errors.timeSlotId && (
          <div className="text-red-500 mt-1">{formik.errors.timeSlotId}</div>
        )}
      </div>

      <CancelAndSubmit onCancel={onCancel} /> 

      {loading && <Loading label="Loading specializations..." />}
      {error && <ErrorBox value={error}></ErrorBox>}
    </form>
  )
}

export { CreateAppointmentForm }