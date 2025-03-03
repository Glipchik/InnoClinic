import Button from "@shared/ui/controls/Button";
import { useState } from "react";
import { CreateSpecializationForm } from "./form";

export const CreateSpecialization = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Specialization
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && <CreateSpecializationForm close={() => setIsCreating(false)} />}
      </div>
    </>
  )
}