import { useContext, useEffect, useState } from "react";
import { SpecializationsList } from "../../features/specializations/SpecializationsList";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { SpecializationForm } from "../../features/specializations/SpecializationForm";
import CreateSpecializationModel from "../../models/specializations/createSpecializationModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";

function SpecializationsPage() {
  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const {
    fetchSpecializationsError, fetchSpecializationsLoading,
    createSpecializationError, createSpecializationLoading,
    editSpecializationError, editSpecializationLoading,
    deleteSpecializationLoading, deleteSpecializationError, createSpecialization } = useSpecializations(token);

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
      
      {fetchSpecializationsLoading && <Loading label="Fetching Specializations: Loading..." />}
      {fetchSpecializationsError && <ErrorBox value={`Fetching Error: ${fetchSpecializationsError}`} />}

      {editSpecializationLoading && <Loading label="Editing Specialization: Editing..." />}
      {editSpecializationError && <ErrorBox value={`Editing Error: ${editSpecializationError}`} />}

      {deleteSpecializationLoading && <Loading label="Deleting Specializations: Deleting..." />}
      {deleteSpecializationError && <ErrorBox value={`Deleting Error: ${deleteSpecializationError}`} />}

      {createSpecializationLoading && <Loading label="Creating Specializations: Creating..." />}
      {createSpecializationError && <ErrorBox value={`Creating Error: ${createSpecializationError}`} />}

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Specialization
        </Button>
      </div>

      <div className="flex flex-col items-center m-4">
        {token && isCreating && <SpecializationForm onCancel={() => setIsCreating(false)} onSubmit={(createSpecializationModel : CreateSpecializationModel) => {
          createSpecialization(createSpecializationModel).then(() => {
            setIsCreating(false)
            setListKey(prevKey => prevKey + 1)
          });
        }} createSpecializationModel={{ specializationName: "", isActive: true } as CreateSpecializationModel} /> }

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