import { useContext, useEffect, useState } from "react";
import { CreateAppointmentForm } from "../../features/appointments/CreateAppointmentForm/index";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { useAppointments } from "../../shared/hooks/useAppointments";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";

function AppointmentsPage() {

  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  
  const { createAppointmentError, createAppointmentLoading, createAppointment } = useAppointments(token)
  

  const userManager = useContext(UserManagerContext)
  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );
  
  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser()
        setToken(user?.access_token ?? null)
      }
      fetchUser()
    }
  }, [userManager, isUserAuthorized])

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Appointments
      </h1> 

      {createAppointmentLoading && <Loading label="Creating Appointment..." />}
      {createAppointmentError && <ErrorBox value={createAppointmentError} />}

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Appointment
        </Button>
      </div>

      <div className="flex flex-col items-center">
        {token && isCreating && <CreateAppointmentForm onSubmit={createAppointment} token={token} /> }
      </div>

    </>

  )
}

export { AppointmentsPage };