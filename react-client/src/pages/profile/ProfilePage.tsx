import { useContext, useEffect, useState } from "react";
import { EditPatientForm } from "../../features/patients";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import Button from "../../shared/ui/controls/Button";
import { useProfiles } from "../../shared/hooks/useProfiles";
import { GETProfilePictureUrl } from "../../shared/api/profileApi";
import profile_pic from "../../assets/profile_pic.png";
import PatientModel from "../../models/patients/PatientModel";
import EditPatientModel from "../../models/patients/EditPatientModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { EditDoctorByDoctorForm } from "../../features/profile";
import DoctorModel from "../../models/doctors/DoctorModel";
import EditDoctorModelByDoctor from "../../models/doctors/EditDoctorModelByDoctor";
import { EditReceptionistForm } from "../../features/receptionists";
import ReceptionistModel from "../../models/receptionists/ReceptionistModel";
import EditReceptionistModel from "../../models/receptionists/EditReceptionistModel";

export default function ProfilePage() {
  const [token, setToken] = useState<string | null>(null);
  const [role, setRole] = useState<string | null>(null);
  const [photoUrl, setPhotoUrl] = useState<string | null>(null);
  const [isEditing, setIsEditing] = useState<boolean>(false);

  const {
    fetchProfileLoading,
    fetchProfileError,
    fetchProfileData,
    editProfileError,
    editProfileLoading,
    deletePatientsProfile,
    deleteProfileError,
    deleteProfileLoading,
    fetchDoctorsProfile,
    fetchPatientsProfile,
    fetchReceptionistsProfile,
    editPatientsProfile,
    editDoctorsProfile,
    editReceptionistsProfile
  } = useProfiles(token);

  const userManager = useContext(UserManagerContext);
  const { isUserAuthorized } = useSelector((state: RootState) => state.auth);

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser();
        setToken(user?.access_token ?? null);
        setRole(user?.profile.role ?? null);
      }
      fetchUser();
    }
  }, [userManager, isUserAuthorized]);

  useEffect(() => {
    if (token && role) {
      if (role === "Patient") {
        fetchPatientsProfile();
      } else if (role === "Doctor") {
        fetchDoctorsProfile();
      } else if (role === "Receptionist") {
        fetchReceptionistsProfile();
      }
    }
  }, [token, role]);

  useEffect(() => {
    if (fetchProfileData?.account?.id && token) {
      GETProfilePictureUrl(fetchProfileData.account.id, token)
        .then((response) => setPhotoUrl(response.data))
        .catch(() => setPhotoUrl(null));
    }
  }, [fetchProfileData, token]);

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">My profile</h1>

      {fetchProfileLoading && <Loading key="fetchProfileLoading" label="Fetching Profile: Loading..." />}
      {fetchProfileError && <ErrorBox key="fetchProfileError" value={`Fetching Error: ${fetchProfileError}`} />}

      {editProfileLoading && <Loading key="editProfileLoading" label="Editing Profile: Editing..." />}
      {editProfileError && <ErrorBox key="editProfileError" value={`Editing Error: ${editProfileError}`} />}

      {deleteProfileLoading && <Loading key="deleteProfileLoading" label="Deleting Profile: Deleting..." />}
      {deleteProfileError && <ErrorBox key="deleteProfileError" value={`Deleting Error: ${deleteProfileError}`} />}

      {role === "Patient" && fetchProfileData && (
        isEditing ? (
          <EditPatientForm
            editPatientModel={{
              id: fetchProfileData.id,
              dateOfBirth: (fetchProfileData as PatientModel).dateOfBirth.toString(),
              firstName: fetchProfileData.firstName,
              lastName: fetchProfileData.lastName,
              middleName: fetchProfileData.middleName,
              photo: null
            }}
            onCancel={() => setIsEditing(false)}
            onSubmit={async (editPatientModel: EditPatientModel) => {
              await editPatientsProfile(editPatientModel);
              setIsEditing(false);
              fetchPatientsProfile();
            }}
          />
        ) : (
          <div className="flex flex-col space-y-3 my-2">
            <div className="flex items-center space-x-4">
              <img
                src={photoUrl || profile_pic}
                alt={fetchProfileData.firstName}
                className="w-16 h-16 rounded-full object-cover"
              />
              <div>
                <h3 className="text-xl font-semibold">
                  {fetchProfileData.firstName} {fetchProfileData.lastName}
                </h3>
                <p className="text-gray-500">{role}</p>
              </div>
            </div>
            <div className="flex flex-col space-y-1">
              <p className="text-lg">Email: {fetchProfileData.account.email}</p>
              <p className="text-lg">Phone: {fetchProfileData.account.phoneNumber}</p>
              <p className="text-lg">Date of Birth: {(fetchProfileData as PatientModel).dateOfBirth.toString()}</p>
            </div>
            <div className="flex justify-start space-x-4 mt-4">
              <Button onClick={() => setIsEditing(true)}>Edit</Button>
              <Button
                onClick={async () => {
                  await deletePatientsProfile(fetchProfileData.id);
                  setIsEditing(false);
                }}
                className="bg-red-600 hover:bg-red-700"
              >
                Delete
              </Button>
            </div>
          </div>
        )
      )}

      {role === "Doctor" && fetchProfileData && (
        isEditing ? (
          <EditDoctorByDoctorForm
            editDoctorModelByDoctor={{
              id: fetchProfileData.id,
              dateOfBirth: (fetchProfileData as DoctorModel).dateOfBirth.toString(),
              careerStartYear: (fetchProfileData as DoctorModel).careerStartYear.toString(),
              firstName: fetchProfileData.firstName,
              lastName: fetchProfileData.lastName,
              middleName: fetchProfileData.middleName,
              photo: null
            }}
            onCancel={() => setIsEditing(false)}
            onSubmit={async (editDoctorModelByDoctor: EditDoctorModelByDoctor) => {
              await editDoctorsProfile(editDoctorModelByDoctor);
              setIsEditing(false);
              fetchPatientsProfile();
            }}
          />
        ) : (
          <div className="flex flex-col space-y-3 my-2">
            <div className="flex items-center space-x-4">
              <img
                src={photoUrl || profile_pic}
                alt={fetchProfileData.firstName}
                className="w-16 h-16 rounded-full object-cover"
              />
              <div>
                <h3 className="text-xl font-semibold">
                  {fetchProfileData.firstName} {fetchProfileData.lastName}
                </h3>
                <p className="text-gray-500">{role}</p>
              </div>
            </div>
            <div className="flex flex-col space-y-1">
              <p className="text-lg">Email: {fetchProfileData.account.email}</p>
              <p className="text-lg">Phone: {fetchProfileData.account.phoneNumber}</p>
              <p className="text-lg">Date of Birth: {(fetchProfileData as PatientModel).dateOfBirth.toString()}</p>
              <p className="text-lg">Career Start: {(fetchProfileData as DoctorModel).careerStartYear.toString()}</p>
              <p className="text-lg">Specialization: {(fetchProfileData as DoctorModel).specialization.specializationName}</p>
              <p className="text-lg">Office: {(fetchProfileData as DoctorModel).office.address}</p>
              <p className="text-lg">Registry Phone: {(fetchProfileData as DoctorModel).office.registryPhoneNumber}</p>
            </div>
            <div className="flex justify-start space-x-4 mt-4">
              <Button onClick={() => setIsEditing(true)}>Edit</Button>
            </div>
          </div>
        )
      )}

      {role === "Receptionist" && fetchProfileData && (
        isEditing ? (
          <EditReceptionistForm
            editReceptionistModel={{
              id: fetchProfileData.id,
              firstName: fetchProfileData.firstName,
              lastName: fetchProfileData.lastName,
              middleName: fetchProfileData.middleName,
              officeId: (fetchProfileData as ReceptionistModel).office.id,
              photo: null
            }}
            token={token}
            onCancel={() => setIsEditing(false)}
            onSubmit={async (editReceptionistModel: EditReceptionistModel) => {
              await editReceptionistsProfile(editReceptionistModel);
              setIsEditing(false);
              fetchPatientsProfile();
            }}
          />
        ) : (
          <div className="flex flex-col space-y-3 my-2">
            <div className="flex items-center space-x-4">
              <img
                src={photoUrl || profile_pic}
                alt={fetchProfileData.firstName}
                className="w-16 h-16 rounded-full object-cover"
              />
              <div>
                <h3 className="text-xl font-semibold">
                  {fetchProfileData.firstName} {fetchProfileData.lastName}
                </h3>
                <p className="text-gray-500">{role}</p>
              </div>
            </div>
            <div className="flex flex-col space-y-1">
              <p className="text-lg">Email: {fetchProfileData.account.email}</p>
              <p className="text-lg">Phone: {fetchProfileData.account.phoneNumber}</p>
              <p className="text-lg">Office: {(fetchProfileData as ReceptionistModel).office.address}</p>
              <p className="text-lg">Registry Phone: {(fetchProfileData as ReceptionistModel).office.registryPhoneNumber}</p>
            </div>
            <div className="flex justify-start space-x-4 mt-4">
              <Button onClick={() => setIsEditing(true)}>Edit</Button>
            </div>
          </div>
        )
      )}
    </>
  );
}