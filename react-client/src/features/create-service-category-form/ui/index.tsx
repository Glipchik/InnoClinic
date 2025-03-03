import Button from "@shared/ui/controls/Button";
import { useState } from "react";
import { CreateServiceCategoryForm } from "./form";

export const CreateServiceCategory = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Service Category
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && <CreateServiceCategoryForm close={() => setIsCreating(false)} />}
      </div>
    </>
  )
}