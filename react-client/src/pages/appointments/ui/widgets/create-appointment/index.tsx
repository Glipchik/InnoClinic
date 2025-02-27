import { CreateAppointmentForm } from "@features/create-appointment-form";
import Button from "@shared/ui/controls/Button";
import { useState } from "react";

const CreateAppointment = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Appointment
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && <CreateAppointmentForm onCancel={() => setIsCreating(false)} />}
      </div>
    </>
  )
}

export default CreateAppointment