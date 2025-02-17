import { useDispatch, useSelector } from "react-redux"
import { DELETE, GET as receptionistGET, POST, PUT, GETWithPagination } from "../api/receptionistApi"
import { fetchReceptionistsFailure, fetchReceptionistsSuccess, fetchReceptionistsRequest } from "../../store/slices/receptionists/fetchReceptionistsSlice"
import { RootState } from "../../store/store";
import CreateReceptionistModel from "../../models/receptionists/CreateReceptionistModel";
import { createReceptionistFailure, createReceptionistRequest, createReceptionistSuccess } from "../../store/slices/receptionists/createReceptionistSlice";
import Receptionist from "../../entities/receptionist";
import { editReceptionistFailure, editReceptionistRequest, editReceptionistSuccess } from "../../store/slices/receptionists/editReceptionistSlice";
import { deleteReceptionistFailure, deleteReceptionistRequest, deleteReceptionistSuccess } from "../../store/slices/receptionists/deleteReceptionistSlice";

export const useReceptionists = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchReceptionistsLoading, error : fetchReceptionistsError, data : fetchReceptionistsData } = useSelector(
    (state: RootState) => state.fetchReceptionistsReducer
  );

  const fetchReceptionists = async () => {
    if (token) {
      try {
        dispatch(fetchReceptionistsRequest())
        const response = await receptionistGET(null, token)
        dispatch(fetchReceptionistsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchReceptionistsFailure(errorMessage))
      }
    }
  }

  const fetchReceptionistsWithPagination = async (officeId: string | null, pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchReceptionistsRequest())
        const response = await GETWithPagination(officeId, pageIndex, pageSize, token)
        dispatch(fetchReceptionistsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchReceptionistsFailure(errorMessage))
      }
    }
  }
  
  const { loading : createReceptionistLoading, error : createReceptionistError } = useSelector(
    (state: RootState) => state.createReceptionistReducer
  );

  const createReceptionist = async (createReceptionistModel: CreateReceptionistModel) => {
    if (token) {
      try {
        dispatch(createReceptionistRequest())
        await POST(createReceptionistModel, token)
        dispatch(createReceptionistSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createReceptionistFailure(errorMessage))
      }
    }
  }

  const { loading : editReceptionistLoading, error : editReceptionistError } = useSelector(
    (state: RootState) => state.editReceptionistReducer
  );

  const editReceptionist = async (receptionist: Receptionist) => {
    if (token) {
      try {
        dispatch(editReceptionistRequest())
        await PUT(receptionist, token)
        dispatch(editReceptionistSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editReceptionistFailure(errorMessage))
      }
    }
  }

  const { loading : deleteReceptionistLoading, error : deleteReceptionistError } = useSelector(
    (state: RootState) => state.deleteReceptionistReducer
  );

  const deleteReceptionist = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteReceptionistRequest())
        await DELETE(id, token)
        dispatch(deleteReceptionistSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deleteReceptionistFailure(errorMessage))
      }
    }
  }

  return {
    fetchReceptionistsLoading, fetchReceptionistsError, fetchReceptionistsData,
    createReceptionistLoading, createReceptionistError,
    editReceptionistLoading, editReceptionistError,
    deleteReceptionistLoading, deleteReceptionistError,
    fetchReceptionists, editReceptionist, createReceptionist, deleteReceptionist, fetchReceptionistsWithPagination }
}