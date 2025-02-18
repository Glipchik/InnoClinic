import { useContext, useEffect, useState } from "react";
import { CreatePatientForm } from "../../features/patients/index"
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { usePatients } from "../../shared/hooks/usePatients";
import CreatePatientModel from "../../models/patients/CreatePatientModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import CreateAccountModelForProfile from "../../models/accounts/CreateAccountModelForProfile";
import { PatientsList } from "../../features/patients/PatientsList";

function PatientsPage() {

  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const [listKey, setListKey] = useState(0);
  
  const {
    fetchPatientsError, fetchPatientsLoading,
    createPatientError, createPatientLoading,
    editPatientError, editPatientLoading,
    deletePatientLoading, deletePatientError, createPatient } = usePatients(token);

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
        Patients
      </h1> 
      
      {fetchPatientsLoading && <Loading label="Fetching Patients: Loading..." />}
      {fetchPatientsError && <ErrorBox value={`Fetching Error: ${fetchPatientsError}`} />}

      {editPatientLoading && <Loading label="Editing Patient: Editing..." />}
      {editPatientError && <ErrorBox value={`Editing Error: ${editPatientError}`} />}

      {deletePatientLoading && <Loading label="Deleting Patients: Deleting..." />}
      {deletePatientError && <ErrorBox value={`Deleting Error: ${deletePatientError}`} />}

      {createPatientLoading && <Loading label="Creating Patients: Creating..." />}
      {createPatientError && <ErrorBox value={`Creating Error: ${createPatientError}`} />}

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Patient
        </Button>
      </div>

      
      <div className="flex flex-col items-center m-4">
        {token && isCreating && <CreatePatientForm onCancel={() => setIsCreating(false)} onSubmit={(createPatientModel : CreatePatientModel) => {
          createPatient(createPatientModel as CreatePatientModel).then(() => {
            setIsCreating(false)
            setListKey(prevKey => prevKey + 1)
          });
        }} createPatientModel={{ 
          firstName: "",
          lastName: "", 
          middleName: "",
          dateOfBirth: (new Date()).toString(),
          account: {
            email: "",
            phoneNumber: "",
          } as CreateAccountModelForProfile,
          photo: null } as CreatePatientModel} /> }
      </div>
      
      <div className="min-h-dvh m-4">
        {token && <PatientsList key={listKey} token={token} />}
      </div>
    </>

  )
}

export { PatientsPage };