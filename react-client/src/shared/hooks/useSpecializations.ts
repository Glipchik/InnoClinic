import { useDispatch, useSelector } from "react-redux"
import { GETWithPagination, POST, PUT, GET as specializationGET } from "../api/specializationApi"
import {
  fetchSpecializationsDataFailure,
  fetchSpecializationsDataSuccess,
  fetchSpecializationsDataRequest,
  fetchPaginatedSpecializationsDataSuccess
} from "../../store/slices/specializationsSlice"
import { RootState } from "../../store/store";
import Specialization from "../../entities/specialization";
import CreateSpecializationModel from "../../features/specializations/models/createSpecializationModel";

export const useSpecializations = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, specializationsData, paginatedSpecializationsData } = useSelector(
    (state: RootState) => state.specializations
  );

  const fetchSpecializations = async () => {
    if (token) {
      try {
        dispatch(fetchSpecializationsDataRequest())
        const response = await specializationGET(null, token)
        dispatch(fetchSpecializationsDataSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(
          fetchSpecializationsDataFailure(errorMessage),
        )
      }
    }
  }

  const fetchSpecializationsWithPagination = async (pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchSpecializationsDataRequest())
        const response = await GETWithPagination(pageIndex, pageSize, token)
        dispatch(fetchPaginatedSpecializationsDataSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(
          fetchSpecializationsDataFailure(errorMessage),
        )
      }
    }
  }

  const createSpecialization = async (createSpecializationModel: CreateSpecializationModel) => {
    if (token) {
      try {
        dispatch(fetchSpecializationsDataRequest())
        await POST(createSpecializationModel, token)
        dispatch(fetchSpecializationsDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchSpecializationsDataFailure(errorMessage))
      }
    }
  }

  const editSpecialization = async (specialization: Specialization) => {
    if (token) {
      try {
        dispatch(fetchSpecializationsDataRequest())
        await PUT(specialization, token)
        dispatch(fetchSpecializationsDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchSpecializationsDataFailure(errorMessage))
      }
    }
  }

  return { loading, error, specializationsData, paginatedSpecializationsData, fetchSpecializations, editSpecialization, createSpecialization, fetchSpecializationsWithPagination }
}

