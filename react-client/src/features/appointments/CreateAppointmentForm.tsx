"use client"

import type React from "react"
import { useState, useEffect, useContext } from "react"
import { useFormik } from "formik"
import { UserManagerContext } from "../../shared/contexts/UserManagerContext"
import { validationSchema } from "./validationSchema"
import { MIN_APPOINTMENT_DATE } from "./helpers/dateUtils"
import { useSpecializations } from "../hooks/useSpecializations"
import { useServices } from "../hooks/useServices"
import { useDoctors } from "../hooks/useDoctors"
import { useDoctorSchedule } from "../hooks/useDoctorSchedule"
import Select from "../../shared/ui/forms/Select"
import DatePicker from "../../shared/ui/forms/DatePicker"
import Button from "../../shared/ui/controls/Button"
import type Specialization from "../../entities/specialization"
import type Service from "../../entities/service"
import type Doctor from "../../entities/doctor"
import type TimeSlot from "../../entities/timeSlot"
import { RootState } from "../../store/store";
import CreateAppointmentModel from "./models/CreateAppointmentModel"
import { fetchAppointmentsDataFailure, fetchAppointmentsDataRequest, fetchAppointmentsDataSuccess } from "../../store/slices/appointmentsSlice"
import { POST as appointmentPOST } from "../../shared/api/appointmentApi"
import { useDispatch, useSelector } from "react-redux"

export function CreateAppointmentForm() {
  const [token, setToken] = useState<string | null>(null)
  const [date, setDate] = useState<string | null>(null)
  const [doctorId, setDoctorId] = useState<string | null>(null)
  const [isServiceSelectDisabled, setIsServiceSelectDisabled] = useState<boolean>(true)
  const [isDoctorSelectDisabled, setIsDoctorSelectDisabled] = useState<boolean>(true)
  const [isTimeSlotSelectDisabled, setIsTimeSlotSelectDisabled] = useState<boolean>(true)
  
  const dispatch = useDispatch()

  const userManager = useContext(UserManagerContext)
  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );

  const { loading: specializationsLoading, error: specializationsError, specializationsData } = useSpecializations(token)
  const { loading: servicesLoading, error: servicesError, servicesData, fetchServices } = useServices(token)
  const { loading: doctorsLoading, error: doctorsError, doctorsData, fetchDoctors } = useDoctors(token)
  const { loading: doctorScheduleLoading, error: doctorScheduleError, doctorScheduleData, fetchDoctorSchedule } =
    useDoctorSchedule(token)

  const { loading: appointmentLoading, error: appointmentError } = useSelector(
    (state: RootState) => state.appointments
  );

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser()
        setToken(user?.access_token ?? null)
      }
      fetchUser()
    }
  }, [userManager, isUserAuthorized])

  const formik = useFormik({
    initialValues: {
      serviceId: "",
      specializationId: "",
      date: MIN_APPOINTMENT_DATE.toISOString().split("T")[0],
      timeSlotId: -1,
      doctorId: "",
    },
    validationSchema,
    onSubmit: (values) => handleSubmit(values),
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
    console.log(doctorId)
    setDoctorId(e.target.value)

    if (doctorId && date) {
      setIsTimeSlotSelectDisabled(false)
      fetchDoctorSchedule(doctorId, new Date(date))
    } else {
      setIsTimeSlotSelectDisabled(true)
    }
  }

  const handleSubmit = async (values: CreateAppointmentModel) => {
    alert(JSON.stringify(values, null, 2))
    if (token) {
      try {
        dispatch(fetchAppointmentsDataRequest())
        const response = await appointmentPOST(values, token)
        dispatch(fetchAppointmentsDataSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
  
        dispatch(fetchAppointmentsDataFailure(errorMessage));      
      }
    }
  }

  return (
    <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 p-4 bg-gray-200 shadow-md rounded-lg w-[50%]">
      {/* Specialization Select */}
      <div className="flex flex-col">
        {specializationsLoading && <p className="text-blue-500">Loading specializations...</p>}
        {specializationsError && <p className="text-red-500">Error: {specializationsError}</p>}
        <Select
          disabled={false}
          label="Specialization"
          id="specializationId"
          name="specializationId"
          onChange={handleSpecializationChange}
          value={formik.values.specializationId}
        >
          <option value="" label="Select specialization" />
          {specializationsData &&
            specializationsData.map((spec: Specialization) => (
              <option key={spec.id} value={spec.id} label={spec.specializationName} />
            ))}
        </Select>
        {formik.touched.specializationId && formik.errors.specializationId ? (
          <div className="text-red-500">{formik.errors.specializationId}</div>
        ) : null}
      </div>

      {/* Service Select */}
      <div className="flex flex-col">
        {servicesLoading && <p className="text-blue-500">Loading services...</p>}
        {servicesError && <p className="text-red-500">Error: {servicesError}</p>}
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
          {servicesData &&
            servicesData.map((service: Service) => (
              <option key={service.id} value={service.id} label={service.serviceName} />
            ))}
        </Select>
        {formik.touched.serviceId && formik.errors.serviceId ? (
          <div className="text-red-500">{formik.errors.serviceId}</div>
        ) : null}
      </div>

      {/* Doctor Select */}
      <div className="flex flex-col">
        {doctorsLoading && <p className="text-blue-500">Loading doctors...</p>}
        {doctorsError && <p className="text-red-500">Error: {doctorsError}</p>}
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
          {doctorsData &&
            doctorsData.map((doctor: Doctor) => (
              <option key={doctor.id} value={doctor.id} label={`${doctor.firstName} ${doctor.lastName}`} />
            ))}
        </Select>
        {formik.touched.doctorId && formik.errors.doctorId ? (
          <div className="text-red-500">{formik.errors.doctorId}</div>
        ) : null}
      </div>

      {/* Date Picker */}
      <DatePicker
        label="Choose a date of an appointment"
        id="date"
        name="date"
        value={formik.values.date}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
          const selectedDate = e.target.value;

          setDate(selectedDate); // Обновляем локальное состояние
          formik.setFieldValue("date", selectedDate); // Уведомляем Formik

          console.log("Выбранная дата:", selectedDate);

          if (doctorId && selectedDate) {
            setIsTimeSlotSelectDisabled(false);
            fetchDoctorSchedule(doctorId, new Date(selectedDate)); // Используем новое значение сразу
          } else {
            setIsTimeSlotSelectDisabled(true);
          }
        }}
        onBlur={formik.handleBlur}
        disabled={false}
        className="bg-gray-100"
      />
      {formik.touched.date && formik.errors.date ? <div className="text-red-500">{formik.errors.date}</div> : null}

      {/* Time Slot Select */}
      <div className="flex flex-col">
        {doctorScheduleLoading && <p className="text-blue-500">Loading Doctor Schedule...</p>}
        {doctorScheduleError && <p className="text-red-500">Error: {doctorScheduleError}</p>}
        <Select
          disabled={isTimeSlotSelectDisabled}
          label="Time Slots"
          id="timeSlotId"
          name="timeSlotId"
          onChange={formik.handleChange}
          value={formik.values.timeSlotId}
          className={isTimeSlotSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select Time Slot" />
          {doctorScheduleData &&
            doctorScheduleData.map((timeSlot: TimeSlot) => (
              <option key={timeSlot.id} value={timeSlot.id} label={timeSlot.start} />
            ))}
        </Select>
        {formik.touched.timeSlotId && formik.errors.timeSlotId ? (
          <div className="text-red-500">{formik.errors.timeSlotId}</div>
        ) : null}
      </div>

      {/* Submit Button */}
      <Button type="submit" className="w-full">
        Submit
      </Button>
      
      {appointmentLoading && <p className="text-blue-500">Creating appointment...</p>}
      {appointmentError && <p className="text-red-500">Error: {appointmentError}</p>}
    </form>
  )
}