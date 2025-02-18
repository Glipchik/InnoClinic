import { useContext, useEffect, useState } from "react";
import { CreateReceptionistForm } from "../../features/receptionists/index"
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { useReceptionists } from "../../shared/hooks/useReceptionists";
import CreateReceptionistModel from "../../models/receptionists/CreateReceptionistModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import CreateAccountModelForProfile from "../../models/accounts/CreateAccountModelForProfile";
import { ReceptionistsList } from "../../features/receptionists/ReceptionistsList";

function ReceptionistsPage() {

  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const [listKey, setListKey] = useState(0);
  
  const {
    fetchReceptionistsError, fetchReceptionistsLoading,
    createReceptionistError, createReceptionistLoading,
    editReceptionistError, editReceptionistLoading,
    deleteReceptionistLoading, deleteReceptionistError, createReceptionist } = useReceptionists(token);

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
        Receptionists
      </h1> 
      
      {fetchReceptionistsLoading && <Loading label="Fetching Receptionists: Loading..." />}
      {fetchReceptionistsError && <ErrorBox value={`Fetching Error: ${fetchReceptionistsError}`} />}

      {editReceptionistLoading && <Loading label="Editing Receptionist: Editing..." />}
      {editReceptionistError && <ErrorBox value={`Editing Error: ${editReceptionistError}`} />}

      {deleteReceptionistLoading && <Loading label="Deleting Receptionists: Deleting..." />}
      {deleteReceptionistError && <ErrorBox value={`Deleting Error: ${deleteReceptionistError}`} />}

      {createReceptionistLoading && <Loading label="Creating Receptionists: Creating..." />}
      {createReceptionistError && <ErrorBox value={`Creating Error: ${createReceptionistError}`} />}

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Receptionist
        </Button>
      </div>

      
      <div className="flex flex-col items-center m-4">
        {token && isCreating && <CreateReceptionistForm
          token={token}
          onCancel={() => setIsCreating(false)}
          onSubmit={(createReceptionistModel : CreateReceptionistModel) => {
            createReceptionist(createReceptionistModel as CreateReceptionistModel).then(() => {
              setIsCreating(false)
              setListKey(prevKey => prevKey + 1)
            });
          }}
          createReceptionistModel={{ 
            firstName: "",
            lastName: "", 
            middleName: "",
            officeId: "", 
            dateOfBirth: (new Date()).toString(),
            account: {
              email: "",
              phoneNumber: "",
            } as CreateAccountModelForProfile,
            photo: null } as CreateReceptionistModel} /> }
      </div>
      
      <div className="min-h-dvh m-4">
        {token && <ReceptionistsList key={listKey} token={token} />}
      </div>
    </>

  )
}

export { ReceptionistsPage };