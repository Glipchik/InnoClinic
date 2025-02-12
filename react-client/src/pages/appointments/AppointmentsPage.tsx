import { useContext, useEffect, useState } from "react";
import { CreateAppointmentForm } from "../../features/appointments/CreateAppointmentForm/index";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";

function AppointmentsPage() {

  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);

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
      <h1>
        Hello from Appointments page
      </h1> 

      <div className="flex justify-end mb-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Service
        </Button>
      </div>

      {token && isCreating && <CreateAppointmentForm token={token} />}
    </>

  )
}

export { AppointmentsPage };