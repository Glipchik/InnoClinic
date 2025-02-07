"use client"

import type React from "react"
import { useState, useEffect, useContext } from "react"
import { useFormik } from "formik"
import { useSelector } from "react-redux"
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

export function CreateAppointmentForm() {
  const [token, setToken] = useState<string | null>(null)
  const [date, setDate] = useState<string | null>(null)
  const [isServiceSelectDisabled, setIsServiceSelectDisabled] = useState<boolean>(true)
  const [isDoctorSelectDisabled, setIsDoctorSelectDisabled] = useState<boolean>(true)
  const [isTimeSlotSelectDisabled, setIsTimeSlotSelectDisabled] = useState<boolean>(true)

  const userManager = useContext(UserManagerContext)
  const isUserAuthorized = useSelector((state) => state.isUserAuthorized)

  const { specializationsLoading, specializationsError, specializationsData } = useSpecializations(token)
  const { servicesLoading, servicesError, servicesData, fetchServices } = useServices(token)
  const { doctorsLoading, doctorsError, doctorsData, fetchDoctors } = useDoctors(token)
  const { doctorScheduleLoading, doctorScheduleError, doctorScheduleData, fetchDoctorSchedule } =
    useDoctorSchedule(token)

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
      service: "",
      specialization: "",
      date: MIN_APPOINTMENT_DATE.toISOString().split("T")[0],
      timeSlot: "",
      doctor: "",
    },
    validationSchema,
    onSubmit: (values) => {
      alert(JSON.stringify(values, null, 2))
    },
  })

  const handleSpecializationChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const specializationId = e.target.value
    formik.setFieldValue("specialization", specializationId)

    if (specializationId) {
      setIsServiceSelectDisabled(false)
      setIsDoctorSelectDisabled(false)
      fetchServices(specializationId)
      fetchDoctors(specializationId)
    } else {
      setIsServiceSelectDisabled(true)
    }
  }

  const handleDoctorChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const doctorId = e.target.value
    formik.setFieldValue("doctor", doctorId)

    if (doctorId && date) {
      setIsTimeSlotSelectDisabled(false)
      fetchDoctorSchedule(doctorId, new Date(date))
    } else {
      setIsTimeSlotSelectDisabled(true)
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
          id="specialization"
          name="specialization"
          onChange={handleSpecializationChange}
          value={formik.values.specialization}
        >
          <option value="" label="Select specialization" />
          {specializationsData &&
            specializationsData.map((spec: Specialization) => (
              <option key={spec.id} value={spec.id} label={spec.specializationName} />
            ))}
        </Select>
        {formik.touched.specialization && formik.errors.specialization ? (
          <div className="text-red-500">{formik.errors.specialization}</div>
        ) : null}
      </div>

      {/* Service Select */}
      <div className="flex flex-col">
        {servicesLoading && <p className="text-blue-500">Loading services...</p>}
        {servicesError && <p className="text-red-500">Error: {servicesError}</p>}
        <Select
          disabled={isServiceSelectDisabled}
          label="Service"
          id="service"
          name="service"
          onChange={formik.handleChange}
          value={formik.values.service}
          className={isServiceSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select service" />
          {servicesData &&
            servicesData.map((service: Service) => (
              <option key={service.id} value={service.id} label={service.serviceName} />
            ))}
        </Select>
        {formik.touched.service && formik.errors.service ? (
          <div className="text-red-500">{formik.errors.service}</div>
        ) : null}
      </div>

      {/* Doctor Select */}
      <div className="flex flex-col">
        {doctorsLoading && <p className="text-blue-500">Loading doctors...</p>}
        {doctorsError && <p className="text-red-500">Error: {doctorsError}</p>}
        <Select
          disabled={isDoctorSelectDisabled}
          label="Doctor"
          id="doctor"
          name="doctor"
          onChange={handleDoctorChange}
          value={formik.values.doctor}
          className={isDoctorSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select doctor" />
          {doctorsData &&
            doctorsData.map((doctor: Doctor) => (
              <option key={doctor.id} value={doctor.id} label={`${doctor.firstName} ${doctor.lastName}`} />
            ))}
        </Select>
        {formik.touched.doctor && formik.errors.doctor ? (
          <div className="text-red-500">{formik.errors.doctor}</div>
        ) : null}
      </div>

      {/* Date Picker */}
      <DatePicker
        label="Choose a date of an appointment"
        id="date"
        name="date"
        value={formik.values.date}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
          const value = e.target.value
          setDate(value)
          formik.setFieldValue("date", value)
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
          id="timeSlot"
          name="timeSlot"
          onChange={formik.handleChange}
          value={formik.values.timeSlot}
          className={isTimeSlotSelectDisabled ? "opacity-50 cursor-not-allowed" : ""}
        >
          <option value="" label="Select Time Slot" />
          {doctorScheduleData &&
            doctorScheduleData.map((timeSlot: TimeSlot) => (
              <option key={timeSlot.id} value={timeSlot.id} label={timeSlot.start} />
            ))}
        </Select>
        {formik.touched.timeSlot && formik.errors.timeSlot ? (
          <div className="text-red-500">{formik.errors.timeSlot}</div>
        ) : null}
      </div>

      {/* Submit Button */}
      <Button type="submit" className="w-full">
        Submit
      </Button>
    </form>
  )
}