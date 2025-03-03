import { EditServiceCategoryForm } from "@features/edit-service-category-form";
import Label from "@shared/ui/containers/Label";
import { useParams } from "react-router-dom";

const EditServiceCategoryPage = () => {
  const { id } = useParams();

  if (!id) return <Label type="error" value="Cannot find id" />;

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Edit ServiceCategory
      </h1>
      <div className="min-h-dvh m-4">
        <EditServiceCategoryForm servicecategoryId={id} />
      </div>
    </>
  )
}

export { EditServiceCategoryPage };