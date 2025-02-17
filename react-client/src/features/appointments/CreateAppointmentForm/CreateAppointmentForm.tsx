import type React from "react"
import { useState, useEffect } from "react"
import { validationSchema } from "./validationSchema"
import { MIN_APPOINTMENT_DATE } from "./helpers/dateUtils"
import { useSpecializations } from "../../../shared/hooks/useSpecializations"
import { useServices } from "../../../shared/hooks/useServices"
import { useDoctors } from "../../../shared/hooks/useDoctors"
import { useDoctorSchedule } from "../../../shared/hooks/useDoctorSchedule"
import Select from "../../../shared/ui/forms/Select"
import DatePicker from "../../../shared/ui/forms/DatePicker"
import Button from "../../../shared/ui/controls/Button"
import type Specialization from "../../../entities/specialization"
import type Service from "../../../entities/service"
import type Doctor from "../../../entities/doctor"
import type TimeSlot from "../../../entities/timeSlot"
import ErrorBox from "../../../shared/ui/containers/ErrorBox"
import Loading from "../../../shared/ui/controls/Loading"
import CreateAppointmentModel from "../../../models/appointments/CreateAppointmentModel"
import { useFormik } from 'formik'

interface CreateAppointmentFormProps {
  onSubmit: (createAppointmentModel: CreateAppointmentModel) => void;
  onCancel: () => void;
  token: string | null;
}

const CreateAppointmentForm = ({ onSubmit, onCancel, token } : CreateAppointmentFormProps) => {
  const [date, setDate] = useState<string | null>(null)
  const [isServiceSelectDisabled, setIsServiceSelectDisabled] = useState<boolean>(true)
  const [isDoctorSelectDisabled, setIsDoctorSelectDisabled] = useState<boolean>(true)
  const [isTimeSlotSelectDisabled, setIsTimeSlotSelectDisabled] = useState<boolean>(true)

  const { fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData, fetchSpecializations } = useSpecializations(token)
  const { fetchServicesLoading, fetchServicesError, fetchServicesData, fetchServices } = useServices(token)
  const { fetchDoctorsLoading, fetchDoctorsData, fetchDoctorsError, fetchDoctors } = useDoctors(token)
  const { fetchDoctorScheduleData, fetchDoctorScheduleError, fetchDoctorScheduleLoading, fetchDoctorSchedule } = useDoctorSchedule(token);
  

  useEffect(() => {
    fetchSpecializations()
  }, [token])

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
      setIsServiceSelectDisabled(false)
      setIsDoctorSelectDisabled(false)

      fetchServices(specializationId)
      fetchDoctors(specializationId)
    } else {
      setIsServiceSelectDisabled(true)
      setIsDoctorSelectDisabled(true)
    }
  }

  const handleDoctorChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const doctorId = e.target.value
    formik.setFieldValue("doctorId", doctorId)

    if (doctorId && date) {
      setIsTimeSlotSelectDisabled(false)
      fetchDoctorSchedule(doctorId, new Date(date))
    } else {
      setIsTimeSlotSelectDisabled(true)
    }
  }

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg mx-auto">

      {/* Specialization Select */}
      <div className="flex flex-col">
        {fetchSpecializationsLoading && <Loading label="Loading specializations..." />}
        {fetchSpecializationsError && <p className="text-red-500">{fetchSpecializationsError}</p>}

        <Select
          disabled={false}
          label="Specialization"
          id="specializationId"
          name="specializationId"
          onChange={handleSpecializationChange}
          value={formik.values.specializationId}
        >
          <option value="" label="Select specialization" />
          {fetchSpecializationsData && 
            (fetchSpecializationsData as Specialization[]).map((spec: Specialization) => (
              <option key={spec.id} value={spec.id} label={spec.specializationName} />
            ))}
        </Select>

        {formik.touched.specializationId && formik.errors.specializationId && (
          <div className="text-red-500 mt-1">{formik.errors.specializationId}</div>
        )}
      </div>

      {/* Service Select */}
      <div className="flex flex-col">
        {fetchServicesLoading && <Loading label="Loading services..." />}
        {fetchServicesError && <ErrorBox value={fetchServicesError} />}

        <Select
          disabled={isServiceSelectDisabled}
          label="Service"
          id="serviceId"
          name="serviceId"
          onChange={formik.handleChange}
          value={formik.values.serviceId}
          className={isServiceSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select service" />
          {fetchServicesData && 
            (fetchServicesData as Service[]).map((service: Service) => (
              <option key={service.id} value={service.id} label={service.serviceName} />
            ))}
        </Select>

        {formik.touched.serviceId && formik.errors.serviceId && (
          <div className="text-red-500 mt-1">{formik.errors.serviceId}</div>
        )}
      </div>

      {/* Doctor Select */}
      <div className="flex flex-col">
        {fetchDoctorsLoading && <Loading label="Loading doctors..." />}
        {fetchDoctorsError && <ErrorBox value={fetchDoctorsError} />}

        <Select
          disabled={isDoctorSelectDisabled}
          label="Doctor"
          id="doctorId"
          name="doctorId"
          onChange={handleDoctorChange}
          value={formik.values.doctorId}
          className={isDoctorSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select doctor" />
          {fetchDoctorsData && 
            (fetchDoctorsData as Doctor[]).map((doctor: Doctor) => (
              <option key={doctor.id} value={doctor.id} label={`${doctor.firstName} ${doctor.lastName}`} />
            ))}
        </Select>

        {formik.touched.doctorId && formik.errors.doctorId && (
          <div className="text-red-500 mt-1">{formik.errors.doctorId}</div>
        )}
      </div>

      {/* Date Picker */}
      <div className="flex flex-col">
        <DatePicker
          label="Choose a date for the appointment"
          id="date"
          name="date"
          value={formik.values.date}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
            const selectedDate = e.target.value;
            setDate(selectedDate);
            formik.setFieldValue("date", selectedDate);

            if (formik.values.doctorId && selectedDate) {
              setIsTimeSlotSelectDisabled(false);
              fetchDoctorSchedule(formik.values.doctorId, new Date(selectedDate));
            } else {
              setIsTimeSlotSelectDisabled(true);
            }
          }}
          disabled={false}
        />

        {formik.touched.date && formik.errors.date && (
          <div className="text-red-500 mt-1">{formik.errors.date}</div>
        )}
      </div>

      {/* Time Slot Select */}
      <div className="flex flex-col">
        {fetchDoctorScheduleLoading && <Loading label="Loading doctor schedule..." />}
        {fetchDoctorScheduleError && <ErrorBox value={fetchDoctorScheduleError} />}

        <Select
          disabled={isTimeSlotSelectDisabled}
          label="Available Time Slots"
          id="timeSlotId"
          name="timeSlotId"
          onChange={formik.handleChange}
          value={formik.values.timeSlotId}
          className={isTimeSlotSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select time slot" />
          {fetchDoctorScheduleData && 
            fetchDoctorScheduleData.map((timeSlot: TimeSlot) => (
              <option key={timeSlot.id} value={timeSlot.id} label={timeSlot.start} />
            ))}
        </Select>

        {formik.touched.timeSlotId && formik.errors.timeSlotId && (
          <div className="text-red-500 mt-1">{formik.errors.timeSlotId}</div>
        )}
      </div>

      <div className="flex flex-row space-x-4">
        {/* Submit Button */}
        <Button type="submit" className="w-full bg-blue-600 text-white hover:bg-blue-700">
          Submit
        </Button>

        {/* Cancel Button */}
        <Button onClick={onCancel} className="w-full bg-gray-600 text-white hover:bg-gray-700">
          Cancel
        </Button>
      </div>
    </form>
  )
}

export { CreateAppointmentForm }