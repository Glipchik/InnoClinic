import { useState } from "react";
import { CreateAppointmentForm } from "../../features/create-appointment-form/index";
import Button from "../../shared/ui/controls/Button";

function AppointmentsPage() {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Appointments
      </h1>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Appointment
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && <CreateAppointmentForm onCancel={() => setIsCreating(false)} /> }
      </div>
    </>
  )
}

export { AppointmentsPage };