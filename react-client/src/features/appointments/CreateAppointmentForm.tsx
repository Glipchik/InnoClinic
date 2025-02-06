"use client"

import type React from "react"
import { useEffect, useState } from "react"
import { useFormik } from "formik"
import { GET as specializationGET } from "../../shared/api/specializationApi"
import { GET as serviceGET } from "../../shared/api/serviceApi"
import { GET as doctorScheduleGET } from "../../shared/api/doctorScheduleApi"
import type { AxiosResponse } from "axios"
import type Specialization from "../../entities/specialization"
import {
  fetchSpecializationsDataFailure,
  fetchSpecializationsDataSuccess,
  fetchSpecializationsDataRequest
} from "../../store/slices/specializationsSlice"

import {
  fetchDoctorScheduleDataDataRequest,
  fetchDoctorScheduleDataFailure,
  fetchDoctorScheduleDataSuccess
} from "../../store/slices/doctorScheduleSlice"

import { useDispatch, useSelector } from "react-redux"
import Button from "../../shared/ui/controls/Button"
import type { RootState } from "../../store/store"
import Select from "../../shared/ui/forms/Select"
import type Service from "../../entities/service"
import {
  fetchServicesDataFailure,
  fetchServicesDataSuccess,
  fetchServicesDataRequest
  } from "../../store/slices/servicesSlice"
import DatePicker from "../../shared/ui/forms/DatePicker"
import * as Yup from "yup"
import TimeSlot from "../../entities/timeSlot"

function addDays(date: Date, days: number) {
  const newDate = new Date(date)
  newDate.setDate(date.getDate() + days)
  return newDate
}

const validationSchema = Yup.object({
  service: Yup.string().required("Service is required"),
  specialization: Yup.string().required("Specialization is required"),
  date: Yup.date().required("Date is required").min(addDays(new Date(), 2), "Date must be at least 2 days from today"),
})

function CreateAppointmentForm() {
  const dispatch = useDispatch()
  const { specializationsLoading, specializationsError, specializationsData } = useSelector(
    (state: RootState) => state.specializations,
  )
  const { servicesLoading, servicesError, servicesData } = useSelector((state: RootState) => state.services)
  const { doctorScheduleLoading, doctorScheduleError, doctorScheduleData } = useSelector((state: RootState) => state.doctorSchedule)
  const [isServiceSelectDisabled, setIsServiceSelectDisabled] = useState<boolean>(true)
  const [isTimeSlotSelectDisabled, setIsTimeSlotSelectDisabled] = useState<boolean>(true)

  useEffect(() => {
    const fetchSpecializations = async () => {
      try {
        dispatch(fetchSpecializationsDataRequest())
        const response: AxiosResponse<Specialization[]> = await specializationGET(null)
        dispatch(fetchSpecializationsDataSuccess(response.data))
      } catch (error) {
        dispatch(fetchSpecializationsDataFailure(error instanceof Error ? error.message : "An unknown error occurred"))
      }
    }

    fetchSpecializations()
  }, [dispatch])

  const formik = useFormik({
    initialValues: {
      service: "",
      specialization: "",
      date: addDays(new Date(), 2).toISOString().split("T")[0],
      timeSlot: ""
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
      fetchServices(specializationId)
    } else {
      setIsServiceSelectDisabled(true)
    }
  }

  const fetchServices = async (specializationId: string) => {
    try {
      dispatch(fetchServicesDataRequest())
      const response: AxiosResponse<Service[]> = await serviceGET(null, specializationId)
      dispatch(fetchServicesDataSuccess(response.data))
    } catch (error) {
      dispatch(fetchServicesDataFailure(error instanceof Error ? error.message : "An unknown error occurred"))
    }
  }

  const fetchDoctorSchedule = async (doctorId: string, date: Date) => {
    try {
      dispatch(fetchDoctorScheduleDataDataRequest())
      const response: AxiosResponse<Service[]> = await doctorScheduleGET(doctorId, date)
      dispatch(fetchDoctorScheduleDataSuccess(response.data))
    } catch (error) {
      dispatch(fetchDoctorScheduleDataFailure(error instanceof Error ? error.message : "An unknown error occurred"))
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
        {servicesLoading && <p className="text-blue-500">Loading doctor...</p>}
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

      {/* Date Picker */}
      <DatePicker
        label="Choose a date of an appointment"
        id="date"
        name="date"
        value={formik.values.date}
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
        disabled={false}
        className="bg-gray-100"
      />
      {formik.touched.date && formik.errors.date ? <div className="text-red-500">{formik.errors.date}</div> : null}

      {/* Time Slol Select */}
      <div className="flex flex-col">
        {doctorScheduleLoading && <p className="text-blue-500">Loading Doctor Schedule...</p>}
        {doctorScheduleError && <p className="text-red-500">Error: {servicesError}</p>}
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

export { CreateAppointmentForm }

