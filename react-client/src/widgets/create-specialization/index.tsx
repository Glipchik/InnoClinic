import { CreateSpecializationForm } from "@features/create-specialization-form/ui";
import { CreateEntity } from "@widgets/create-entity";

export const CreateSpecialization = () => (
  <CreateEntity
    buttonText="Create New Specialization"
    renderForm={(close) => <CreateSpecializationForm close={close} />}
  />
);