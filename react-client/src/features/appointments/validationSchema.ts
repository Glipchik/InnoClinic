import * as Yup from 'yup';
import { MIN_APPOINTMENT_DATE } from './helpers/dateUtils';

export const validationSchema = Yup.object({
  service: Yup.string().required("Service is required"),
  doctor: Yup.string().required("Doctor is required"),
  timeSlot: Yup.string().required("Time slot is required"),
  specialization: Yup.string().required("Specialization is required"),
  date: Yup.date().required("Date is required").min(MIN_APPOINTMENT_DATE, "Date must be at least 2 days from today"),
});
