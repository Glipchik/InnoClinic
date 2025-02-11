import { useDispatch, useSelector } from "react-redux"
import { GET as doctorGET } from "../api/doctorApi"
import { fetchDoctorsFailure, fetchDoctorsSuccess, fetchDoctorsRequest } from "../../store/slices/doctors/fetchDoctorsSlice"
import { RootState } from "../../store/store";

export const useDoctors = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchDoctorsLoading, error : fetchDoctorsError, data : fetchDoctorsData } = useSelector(
    (state: RootState) => state.fetchDoctorsReducer
  );

  const fetchDoctors = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchDoctorsRequest())
        const response = await doctorGET(null, specializationId, token)
        dispatch(fetchDoctorsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchDoctorsFailure(errorMessage))
      }
    }
  }

  return { fetchDoctorsLoading, fetchDoctorsError, fetchDoctorsData, fetchDoctors }
}