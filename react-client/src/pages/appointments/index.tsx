import { useState } from "react";
import CreateAppointment from "./ui/widgets/create-appointment";

const AppointmentsPage = () => {
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Appointments
      </h1>
      <CreateAppointment onClick={() => setIsCreating(true)} onCancel={() => setIsCreating(false)} isDisabled={isCreating} />
    </>
  )
}

export { AppointmentsPage };