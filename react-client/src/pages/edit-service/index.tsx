import { EditServiceForm } from "@features/edit-service-form";
import Label from "@shared/ui/containers/Label";
import { useParams } from "react-router-dom";

const EditServicePage = () => {
  const { id } = useParams();

  if (!id) return <Label type="error" value="Cannot find id" />;

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Edit Service
      </h1>
      <div className="min-h-dvh m-4">
        <EditServiceForm serviceId={id} />
      </div>
    </>
  )
}

export { EditServicePage };