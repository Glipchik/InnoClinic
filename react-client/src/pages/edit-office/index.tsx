import { EditOfficeForm } from "@features/edit-office-form/ui";
import Label from "@shared/ui/containers/Label";
import { useParams } from "react-router-dom";

const EditOfficePage = () => {
  const { id } = useParams();

  if (!id) return <Label type="error" value="Cannot find id" />;

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Edit Office
      </h1>
      <div className="min-h-dvh m-4">
        <EditOfficeForm officeId={id} />
      </div>
    </>
  )
}

export { EditOfficePage };