import Button from "@shared/ui/controls/Button";
import { useState } from "react";
import { CreateServiceForm } from "./form";

const CreateService = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Service
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && <CreateServiceForm close={() => setIsCreating(false)} />}
      </div>
    </>
  )
}

export default CreateService