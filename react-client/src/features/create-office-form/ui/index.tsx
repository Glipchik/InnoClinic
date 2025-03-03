import Button from "@shared/ui/controls/Button";
import { useState } from "react";
import { CreateOfficeForm } from "./form";

export const CreateOffice = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Office
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && <CreateOfficeForm close={() => setIsCreating(false)} />}
      </div>
    </>
  )
}