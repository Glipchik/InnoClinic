import { CreateOfficeForm } from "@features/create-office-form/ui";
import { CreateEntity } from "@widgets/create-entity";

export const CreateOffice = () => (
  <CreateEntity
    buttonText="Create New Office"
    renderForm={(close) => <CreateOfficeForm close={close} />}
  />
);
