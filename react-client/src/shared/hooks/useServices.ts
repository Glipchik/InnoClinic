import { useDispatch, useSelector } from "react-redux"
import { GET } from "../api/serviceApi"
import {
  fetchServicesFailure,
  fetchServicesSuccess,
  fetchServicesRequest,
} from "../../store/slices/services/fetchServicesSlice"
import { RootState } from "../../store/store";

export const useServices = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchServicesLoading, error : fetchServicesError, data : fetchServicesData } = useSelector(
    (state: RootState) => state.fetchServicesReducer
  );
  
  const fetchServices = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchServicesRequest())
        const response = await GET(null, specializationId, token)
        dispatch(fetchServicesSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchServicesFailure(errorMessage))
      }
    }
  }

  return { fetchServicesLoading, fetchServicesError, fetchServicesData, fetchServices }
}