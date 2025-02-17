import { useContext, useEffect, useState } from "react";
import { DoctorForm } from "../../features/doctors/index"
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { useDoctors } from "../../shared/hooks/useDoctors";
import CreateDoctorModel from "../../models/doctors/CreateDoctorModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { DoctorStatus } from "../../entities/enums/doctorStatus";
import CreateAccountModelForProfile from "../../models/accounts/CreateAccountModelForProfile";
import { DoctorsList } from "../../features/doctors/DoctorsList";

function DoctorsPage() {

  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const [listKey, setListKey] = useState(0);

  
  const {
    fetchDoctorsError, fetchDoctorsLoading,
    createDoctorError, createDoctorLoading,
    editDoctorError, editDoctorLoading,
    deleteDoctorLoading, deleteDoctorError, createDoctor } = useDoctors(token);

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
        Doctors
      </h1> 
      
      {fetchDoctorsLoading && <Loading label="Fetching Doctors: Loading..." />}
      {fetchDoctorsError && <ErrorBox value={`Fetching Error: ${fetchDoctorsError}`} />}

      {editDoctorLoading && <Loading label="Editing Doctor: Editing..." />}
      {editDoctorError && <ErrorBox value={`Editing Error: ${editDoctorError}`} />}

      {deleteDoctorLoading && <Loading label="Deleting Doctors: Deleting..." />}
      {deleteDoctorError && <ErrorBox value={`Deleting Error: ${deleteDoctorError}`} />}

      {createDoctorLoading && <Loading label="Creating Doctors: Creating..." />}
      {createDoctorError && <ErrorBox value={`Creating Error: ${createDoctorError}`} />}

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Doctor
        </Button>
      </div>

      
      <div className="flex flex-col items-center m-4">
        {token && isCreating && <DoctorForm onCancel={() => setIsCreating(false)} onSubmit={(createDoctorModel : CreateDoctorModel) => {
          createDoctor(createDoctorModel as CreateDoctorModel).then(() => {
            setIsCreating(false)
            setListKey(prevKey => prevKey + 1)
          });
        }} createDoctorModel={{ 
          firstName: "",
          lastName: "", 
          middleName: "",
          officeId: "", 
          specializationId: "", 
          status: DoctorStatus.AtWork,  
          dateOfBirth: (new Date()).toString(), 
          careerStartYear: (new Date()).toString(),
          account: {
            email: "",
            phoneNumber: "",
          } as CreateAccountModelForProfile,
          photo: null } as CreateDoctorModel} /> }

        {createDoctorLoading && <Loading label="Creating Doctor..." />}
        {createDoctorError && <ErrorBox value={createDoctorError} />}
      </div>
      
      <div className="min-h-dvh m-4">
        {token && <DoctorsList key={listKey} token={token} />}
      </div>
    </>

  )
}

export { DoctorsPage };