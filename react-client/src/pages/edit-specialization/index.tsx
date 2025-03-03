import { EditSpecializationForm } from "@features/edit-specialization-form";
import Label from "@shared/ui/containers/Label";
import { useParams } from "react-router-dom";

const EditSpecializationPage = () => {
  const { id } = useParams();

  if (!id) return <Label type="error" value="Cannot find id" />;

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Edit Specialization
      </h1>
      <div className="min-h-dvh m-4">
        <EditSpecializationForm specializationId={id} />
      </div>
    </>
  )
}

export { EditSpecializationPage };