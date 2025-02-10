import { useDispatch, useSelector } from "react-redux"
import { GET as serviceGET } from "../api/serviceApi"
import {
  fetchServicesDataFailure,
  fetchServicesDataSuccess,
  fetchServicesDataRequest,
} from "../../store/slices/servicesSlice"
import { RootState } from "../../store/store";

export const useServices = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, servicesData } = useSelector(
    (state: RootState) => state.services
  );
  
  const fetchServices = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchServicesDataRequest())
        const response = await serviceGET(null, specializationId, token)
        dispatch(fetchServicesDataSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchServicesDataFailure(errorMessage))
      }
    }
  }

  return { loading, error, servicesData, fetchServices }
}

