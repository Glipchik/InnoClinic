import { useDispatch, useSelector } from "react-redux"
import { DELETE, GET, GETWithPagination, POST, PUT } from "../api/specializationApi"

import {
  fetchSpecializationsFailure,
  fetchSpecializationsSuccess,
  fetchSpecializationsRequest,
} from "../../store/slices/specializations/fetchSpecializationsSlice"

import {
  createSpecializationFailure,
  createSpecializationRequest,
  createSpecializationSuccess,
} from "../../store/slices/specializations/createSpecializationSlice"

import {
  editSpecializationFailure,
  editSpecializationRequest,
  editSpecializationSuccess,
} from "../../store/slices/specializations/editSpecializationSlice"

import {
  deleteSpecializationFailure,
  deleteSpecializationRequest,
  deleteSpecializationSuccess,
} from "../../store/slices/specializations/deleteSpecializationSlice"


import { RootState } from "../../store/store";
import Specialization from "../../entities/specialization";
import CreateSpecializationModel from "../../features/specializations/models/createSpecializationModel";

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

  const fetchSpecializationsWithPagination = async (pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchSpecializationsRequest())
        const response = await GETWithPagination(pageIndex, pageSize, token)
        dispatch(fetchSpecializationsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(
          fetchSpecializationsFailure(errorMessage),
        )
      }
    }
  }

  const { loading : createSpecializationLoading, error : createSpecializationError } = useSelector(
    (state: RootState) => state.createSpecializationReducer
  );

  const createSpecialization = async (createSpecializationModel: CreateSpecializationModel) => {
    if (token) {
      try {
        dispatch(createSpecializationRequest())
        await POST(createSpecializationModel, token)
        dispatch(createSpecializationSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createSpecializationFailure(errorMessage))
      }
    }
  }
  
  const { loading : editSpecializationLoading, error : editSpecializationError } = useSelector(
    (state: RootState) => state.editSpecializationReducer
  );

  const editSpecialization = async (specialization: Specialization) => {
    if (token) {
      try {
        dispatch(editSpecializationRequest())
        await PUT(specialization, token)
        dispatch(editSpecializationSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editSpecializationFailure(errorMessage))
      }
    }
  }

  const { loading : deleteSpecializationLoading, error : deleteSpecializationError } = useSelector(
    (state: RootState) => state.deleteSpecializationReducer
  );

  const deleteSpecialization = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteSpecializationRequest())
        await DELETE(id, token)
        dispatch(deleteSpecializationSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deleteSpecializationFailure(errorMessage))
      }
    }
  }

  return {
    fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData,
    createSpecializationLoading, createSpecializationError,
    editSpecializationLoading, editSpecializationError,
    deleteSpecializationLoading, deleteSpecializationError,
    deleteSpecialization,
    createSpecialization,
    editSpecialization, 
    fetchSpecializationsWithPagination, 
    fetchSpecializations }
}
