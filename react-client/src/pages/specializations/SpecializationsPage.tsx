import { useContext, useEffect, useState } from "react";
import { SpecializationsList } from "../../features/specializations/SpecializationsList";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { SpecializationForm } from "../../features/specializations/SpecializationForm";
import Specialization from "../../entities/specialization";
import CreateSpecializationModel from "../../features/specializations/models/createSpecializationModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";

function SpecializationsPage() {
  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const { createSpecializationLoading, createSpecializationError, createSpecialization } = useSpecializations(token);
  const [listKey, setListKey] = useState(0);

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
        Specializations
      </h1> 

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Specialization
        </Button>
      </div>

      <div className="flex flex-col items-center m-4">
        {token && isCreating && <SpecializationForm onCancel={() => setIsCreating(false)} onSubmit={(specialization : Specialization) => {
          createSpecialization(specialization as CreateSpecializationModel).then(() => {
            setIsCreating(false)
            setListKey(prevKey => prevKey + 1)
          });
        }} specialization={{ id: "", specializationName: "", isActive: true } as Specialization} /> }

        {createSpecializationLoading && <Loading label="Creating Specialization..." />}
        {createSpecializationError && <ErrorBox value={createSpecializationError} />}
      </div>
      
      <div className="min-h-dvh m-4">
        {token && <SpecializationsList key={listKey} token={token} />}
      </div>
    </>
  )
}

export { SpecializationsPage };