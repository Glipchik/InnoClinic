import * as Yup from 'yup';
import { MIN_APPOINTMENT_DATE } from '../lib/dateUtils';

export const validationSchema = Yup.object({
  serviceId: Yup.string().required("Service is required"),
  doctorId: Yup.string().required("Doctor is required"),
  timeSlotId: Yup.number().required("Time slot is required").moreThan(0, "Time slot is required"),
  specializationId: Yup.string().required("Specialization is required"),
  date: Yup.date().required("Date is required").min(MIN_APPOINTMENT_DATE, "Date must be at least 2 days from today"),
});
