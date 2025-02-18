import { useDispatch, useSelector } from "react-redux"
import { fetchProfileFailure, fetchProfileRequest, fetchProfileSuccess } from "../../store/slices/profile/fetchProfileSlice"
import { RootState } from "../../store/store";
import { GETDoctorsProfile, GETPatientsProfile, GETReceptionistsProfile } from "../api/profileApi";
import EditDoctorModelByDoctor from "../../models/doctors/EditDoctorModelByDoctor";
import { PUTAsDoctor } from "../api/doctorApi";
import { editProfileFailure, editProfileRequest, editProfileSuccess } from "../../store/slices/profile/editProfileSlice";
import EditPatientModel from "../../models/patients/EditPatientModel";
import { DELETE, PUTAsPatient } from "../api/patientApi";
import EditReceptionistModel from "../../models/receptionists/EditReceptionistModel";
import { PUT } from "../api/receptionistApi";
import { deleteProfileFailure, deleteProfileRequest, deleteProfileSuccess } from "../../store/slices/profile/deleteProfileSlice";

export const useProfiles = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchProfileLoading, error : fetchProfileError, data : fetchProfileData } = useSelector(
    (state: RootState) => state.fetchProfileReducer
  );

  const fetchReceptionistsProfile = async () => {
    if (token) {
      try {
        dispatch(fetchProfileRequest())
        const response = await GETReceptionistsProfile(token)
        dispatch(fetchProfileSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchProfileFailure(errorMessage))
      }
    }
  }

  const fetchDoctorsProfile = async () => {
    if (token) {
      try {
        dispatch(fetchProfileRequest())
        const response = await GETDoctorsProfile(token)
        dispatch(fetchProfileSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchProfileFailure(errorMessage))
      }
    }
  }

  const fetchPatientsProfile = async () => {
    if (token) {
      try {
        dispatch(fetchProfileRequest())
        const response = await GETPatientsProfile(token)
        console.log(response.data)
        dispatch(fetchProfileSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchProfileFailure(errorMessage))
      }
    }
  }

  const { loading : editProfileLoading, error : editProfileError } = useSelector(
    (state: RootState) => state.editProfileReducer
  );

  const editDoctorsProfile = async (editDoctorModelByDoctor: EditDoctorModelByDoctor) => {
    if (token) {
      try {
        dispatch(editProfileRequest())
        await PUTAsDoctor(editDoctorModelByDoctor, token)
        dispatch(editProfileSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editProfileFailure(errorMessage))
      }
    }
  }

  const editPatientsProfile = async (editPatientModel: EditPatientModel) => {
    if (token) {
      try {
        dispatch(editProfileRequest())
        await PUTAsPatient(editPatientModel, token)
        dispatch(editProfileSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editProfileFailure(errorMessage))
      }
    }
  }

  const editReceptionistsProfile = async (editReceptionistModel: EditReceptionistModel) => {
    if (token) {
      try {
        dispatch(editProfileRequest())
        await PUT(editReceptionistModel, token)
        dispatch(editProfileSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editProfileFailure(errorMessage))
      }
    }
  }

  const { loading : deleteProfileLoading, error : deleteProfileError } = useSelector(
    (state: RootState) => state.deleteProfileReducer
  );

  const deletePatientsProfile = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteProfileRequest())
        await DELETE(id, token)
        dispatch(deleteProfileSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deleteProfileFailure(errorMessage))
      }
    }
  }

  return { 
    fetchProfileLoading, fetchProfileError, fetchProfileData, 
    fetchDoctorsProfile,
    fetchPatientsProfile,
    fetchReceptionistsProfile,
    editProfileLoading, editProfileError, editDoctorsProfile, editPatientsProfile, editReceptionistsProfile,
    deleteProfileLoading, deleteProfileError, deletePatientsProfile
  }
}