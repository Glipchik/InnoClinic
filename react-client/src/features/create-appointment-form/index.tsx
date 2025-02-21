import type React from "react"
import { useState, useEffect } from "react"
import { validationSchema } from "./models/validationSchema"
import { MIN_APPOINTMENT_DATE } from "./lib/dateUtils"
import DatePicker from "../../shared/ui/forms/DatePicker"
import { Form, Formik } from 'formik'
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

interface CreateAppointmentFormModel {
  specializationId: string,
  doctorId: string,
  serviceId: string,
  timeSlotId: number,
  date: string
}

const CreateAppointmentForm = ({ onCancel }: CreateAppointmentFormProps) => {
  const dispatch = useDispatch();
  const [date, setDate] = useState<string | null>(null);
  const { loading, error } = useSelector(
    (state: RootState) => state.createAppointment
  );

  useEffect(() => {
    dispatch(fetchSpecializationsRequest());
  }, [dispatch]);

  const initialValues: CreateAppointmentFormModel = {
    serviceId: "",
    specializationId: "",
    date: MIN_APPOINTMENT_DATE.toISOString().split("T")[0],
    timeSlotId: -1,
    doctorId: "",
  };

  const handleSpecializationChange = (e: React.ChangeEvent<HTMLSelectElement>, setFieldValue: any) => {
    const specializationId = e.target.value;
    setFieldValue("specializationId", specializationId, true);
    if (specializationId) {
      dispatch(fetchServicesRequest(specializationId));
      dispatch(fetchDoctorsRequest(specializationId));
    }
  };

  const handleDoctorChange = (e: React.ChangeEvent<HTMLSelectElement>, setFieldValue: any) => {
    const doctorId = e.target.value;
    setFieldValue("doctorId", doctorId, true);
    if (doctorId && date) {
      dispatch(fetchDoctorScheduleRequest({ doctorId, date: new Date(date) }));
    }
  };

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>, setFieldValue: any, doctorId?: string) => {
    const selectedDate = e.target.value;
    setDate(selectedDate);
    setFieldValue("date", selectedDate, true);
    if (doctorId && selectedDate) {
      dispatch(fetchDoctorScheduleRequest({ doctorId, date: new Date(selectedDate) }));
    }
  };

  const onSubmit = (values: CreateAppointmentModel) => {
    dispatch(createAppointmentRequest(values));
  };

  return (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
      {({ values, touched, errors, handleChange, setFieldValue }) => (
        <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
          <SpecializationSelect
            disabled={false}
            id="specialization-select-for-create-appointment-form-id"
            name="specializationId"
            value={values.specializationId}
            onChange={(e) => handleSpecializationChange(e, setFieldValue)}
            error={touched.specializationId ? errors.specializationId : undefined}
          />
          <ServiceSelect
            id="service-select-for-create-appointment-form-id"
            disabled={values.specializationId ? false : true}
            name="serviceId"
            onChange={handleChange}
            value={values.serviceId}
            className={values.specializationId ? "" : "opacity-50 cursor-not-allowed"}
            error={touched.serviceId ? errors.serviceId : undefined}
          />
          <DoctorSelect 
            id="doctor-select-for-create-appointment-form-id"
            disabled={values.specializationId ? false : true}
            name="doctorId"
            onChange={(e) => handleDoctorChange(e, setFieldValue)}
            value={values.doctorId}
            className={values.specializationId ? "" : "opacity-50 cursor-not-allowed"}
            error={touched.doctorId ? errors.doctorId : undefined}
          />
          <DatePicker
            label="Choose a date for the appointment"
            id="select-date-for-create-appointment-form-id"
            name="date"
            value={values.date}
            onChange={(e) => handleDateChange(e, setFieldValue, values.doctorId)}
            disabled={false}
            error={touched.date ? errors.date : undefined}
          />
          <TimeSlotSelect
            id="time-slot-select-for-create-appointment-form-id"
            disabled={(values.date && values.doctorId) ? false : true}
            name="timeSlotId"
            onChange={handleChange}
            value={values.timeSlotId}
            className={(values.date && values.doctorId) ? "" : "opacity-50 cursor-not-allowed"}
            error={touched.timeSlotId ? errors.timeSlotId : undefined}
          />
          <CancelAndSubmit onCancel={onCancel} /> 
          {loading && <Loading label="Loading specializations..." />}
          {error && <ErrorBox value={error}></ErrorBox>}
        </Form>
      )}
    </Formik>
  );
};

export { CreateAppointmentForm };
