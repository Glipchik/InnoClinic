import { useDispatch, useSelector } from "react-redux"
import { GET as doctorGET } from "../../shared/api/doctorApi"
import { fetchDoctorsDataFailure, fetchDoctorsDataSuccess, fetchDoctorsDataRequest } from "../../store/slices/doctorsSlice"
import { RootState } from "../../store/store";

export const useDoctors = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, doctorsData } = useSelector(
    (state: RootState) => state.doctors
  );

  const fetchDoctors = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchDoctorsDataRequest())
        const response = await doctorGET(null, specializationId, token)
        dispatch(fetchDoctorsDataSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchDoctorsDataFailure(errorMessage))
      }
    }
  }

  return { loading, error, doctorsData, fetchDoctors }
}