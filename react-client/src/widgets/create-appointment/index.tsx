import { CreateAppointmentForm } from "@features/create-appointment-form";
import { CreateEntity } from "@widgets/create-entity";

export const CreateAppointment = () => (
  <CreateEntity
    buttonText="Create New Appointment"
    renderForm={(close) => <CreateAppointmentForm onCancel={close} />}
  />
);