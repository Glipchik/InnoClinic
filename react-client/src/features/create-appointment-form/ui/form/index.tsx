import { Formik } from "formik";
import { useDispatch } from "react-redux";
import { MIN_APPOINTMENT_DATE } from "../../lib/dateUtils";
import CreateAppointmentModel from "../../models/createAppointmentModel";
import { validationSchema } from "../../models/validationSchema";
import { createAppointmentRequest } from "../../store/create-appointment";
import InnerForm from "../inner-form";

interface CreateAppointmentFormProps {
  onCancel: () => void;
}

export interface CreateAppointmentFormModel {
  specializationId: string;
  doctorId: string;
  serviceId: string;
  timeSlotId: number;
  date: string;
}

const CreateAppointmentForm = ({ onCancel }: CreateAppointmentFormProps) => {
  const dispatch = useDispatch();

  const initialValues: CreateAppointmentFormModel = {
    serviceId: "",
    specializationId: "",
    date: MIN_APPOINTMENT_DATE.toISOString().split("T")[0],
    timeSlotId: -1,
    doctorId: "",
  };

  const onSubmit = (values: CreateAppointmentModel) => {
    console.log(values)
    dispatch(createAppointmentRequest(values));
  };

  return (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit}>
      <InnerForm onCancel={onCancel} />
    </Formik>
  );
};

export { CreateAppointmentForm };