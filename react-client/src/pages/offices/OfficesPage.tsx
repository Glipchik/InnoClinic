import { useContext, useEffect, useState } from "react";
import { OfficesList } from "../../features/offices/OfficesList";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import Button from "../../shared/ui/controls/Button";
import { OfficeForm } from "../../features/offices/OfficeForm";
import Office from "../../entities/office";
import { useOffices } from "../../shared/hooks/useOffices";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import Loading from "../../shared/ui/controls/Loading";
import CreateOfficeModel from "../../features/offices/models/CreateOfficeModel";

function OfficesPage() {
  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const { createOfficeLoading, createOfficeError, createOffice } = useOffices(token);

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
        Offices
      </h1> 

      <div className="flex justify-end mb-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Office
        </Button>
      </div>

      <div className="flex flex-col items-center">
        {token && isCreating && <OfficeForm onCancel={() => setIsCreating(false)} onSubmit={(office : Office) => {
          createOffice(office as CreateOfficeModel);
        }} office={{ id: "", address: "", registryPhoneNumber: "", isActive: true } as Office} /> }

        {createOfficeLoading && <Loading label="Creating Office..." />}
        {createOfficeError && <ErrorBox value={createOfficeError} />}
      </div>
  
      <div className="min-h-dvh">
        {token && <OfficesList token={token} />}
      </div>
    </>
  )
}

export { OfficesPage };