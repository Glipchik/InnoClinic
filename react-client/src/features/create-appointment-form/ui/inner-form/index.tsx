import { RootState } from "@app/store";
import Label from "@shared/ui/containers/Label";
import Loading from "@shared/ui/controls/Loading";
import DatePicker from "@shared/ui/forms/DatePicker";
import SpecializationSelect from "@shared/ui/specialization-select";
import { useFormikContext } from "formik";
import { useSelector } from "react-redux";
import { Form } from "react-router-dom";
import DoctorSelect from "../doctor-select";
import ServiceSelect from "../service-select";
import TimeSlotSelect from "../time-slot-select";
import { CreateAppointmentFormModel } from "..";
import FormFooter from "@widgets/form-footer";

const InnerForm = ({ onCancel }: { onCancel: () => void }) => {
  const { values, touched, errors, handleChange } = useFormikContext<CreateAppointmentFormModel>();
  const { loading, error, success } = useSelector((state: RootState) => state.createAppointment);

  return (
    <Form className="flex w-[40%] flex-col gap-6 p-6 bg-white shadow-lg rounded-lg max-w-lg m-6">
      <SpecializationSelect
        disabled={false}
        id="specialization-select-for-create-appointment-form"
        name="specializationId"
        value={values.specializationId}
        onChange={handleChange}
        error={touched.specializationId ? errors.specializationId : undefined}
        isLoadingRequired={true}
      />
      <ServiceSelect
        id="service-select-for-create-appointment-form"
        disabled={values.specializationId ? false : true}
        name="serviceId"
        onChange={handleChange}
        value={values.serviceId}
        className={values.specializationId ? "" : "opacity-50 cursor-not-allowed"}
        error={touched.serviceId ? errors.serviceId : undefined}
        specializationId={values.specializationId}
        isLoadingRequired={!!values.specializationId}
      />
      <DoctorSelect
        id="doctor-select-for-create-appointment-form"
        disabled={values.specializationId ? false : true}
        name="doctorId"
        onChange={handleChange}
        value={values.doctorId}
        className={values.specializationId ? "" : "opacity-50 cursor-not-allowed"}
        error={touched.doctorId ? errors.doctorId : undefined}
        isLoadingRequired={!!values.specializationId}
        specializationId={values.specializationId}
      />
      <DatePicker
        label="Choose a date for the appointment"
        id="select-date-for-create-appointment-form"
        name="date"
        value={values.date}
        onChange={handleChange}
        disabled={false}
        error={touched.date ? errors.date : undefined}
      />
      <TimeSlotSelect
        id="time-slot-select-for-create-appointment-form"
        disabled={(values.date && values.doctorId) ? false : true}
        name="timeSlotId"
        onChange={handleChange}
        value={values.timeSlotId}
        className={(values.date && values.doctorId) ? "" : "opacity-50 cursor-not-allowed"}
        error={touched.timeSlotId ? errors.timeSlotId : undefined}
        date={values.date}
        doctorId={values.doctorId}
        isLoadingRequired={!!values.doctorId && !!values.date}
      />
      <FormFooter onCancel={onCancel} />
      {loading && <Loading label="Loading specializations..." />}
      {error && <Label value={error} type="error"></Label>}
      {success && <Label value="Appointment was created successfully" type="success"></Label>}
    </Form>
  );
};

export default InnerForm