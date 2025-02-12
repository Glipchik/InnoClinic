import { useDispatch, useSelector } from "react-redux"
import { GET } from "../api/specializationApi"
import {
  fetchSpecializationsFailure,
  fetchSpecializationsSuccess,
  fetchSpecializationsRequest,
} from "../../store/slices/specializations/fetchSpecializationsSlice"
import { RootState } from "../../store/store";

export const useSpecializations = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchSpecializationsLoading, error : fetchSpecializationsError, data : fetchSpecializationsData } = useSelector(
    (state: RootState) => state.fetchSpecializationsReducer
  );

  const fetchSpecializations = async () => {
    if (token) {
      try {
        dispatch(fetchSpecializationsRequest())
        const response = await GET(null, token)
        dispatch(fetchSpecializationsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchSpecializationsFailure(errorMessage))
      }
    }
  }

  return { fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData, fetchSpecializations }
}