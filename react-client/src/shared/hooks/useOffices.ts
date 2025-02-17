import { useDispatch, useSelector } from "react-redux"
import { GET, GETWithPagination, PUT, POST, DELETE } from "../api/officeApi"
import { fetchOfficesFailure, fetchOfficesSuccess, fetchOfficesRequest } from "../../store/slices/offices/fetchOfficesSlice"
import { createOfficeFailure, createOfficeSuccess, createOfficeRequest } from "../../store/slices/offices/createOfficeSlice"
import { RootState } from "../../store/store";
import CreateOfficeModel from "../../models/offices/CreateOfficeModel";
import { editOfficeFailure, editOfficeRequest, editOfficeSuccess } from "../../store/slices/offices/editOfficeSlice";
import { deleteOfficeFailure, deleteOfficeRequest, deleteOfficeSuccess } from "../../store/slices/offices/deleteOfficeSlice";
import { EditOfficeModel } from "../../models/offices/EditOfficeModel";

export const useOffices = (token: string | null) => {
  const dispatch = useDispatch()

  const { loading : fetchOfficesLoading, error : fetchOfficesError, officesData: fetchOfficesData } = useSelector(
    (state: RootState) => state.fetchOfficesReducer
  );

  const fetchOfficesWithPagination = async (pageIndex: number | null, padeSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchOfficesRequest())
        const response = await GETWithPagination(pageIndex, padeSize, token)
        dispatch(fetchOfficesSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchOfficesFailure(errorMessage))
      }
    }
  }

  const fetchOffices = async () => {
    if (token) {
      try {
        dispatch(fetchOfficesRequest())
        const response = await GET(token)
        dispatch(fetchOfficesSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchOfficesFailure(errorMessage))
      }
    }
  }

  const { loading : createOfficeLoading, error : createOfficeError } = useSelector(
    (state: RootState) => state.createOfficeReducer
  );

  const createOffice = async (createOfficeModel: CreateOfficeModel) => {
    if (token) {
      try {
        dispatch(createOfficeRequest())
        await POST(createOfficeModel, token)
        dispatch(createOfficeSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createOfficeFailure(errorMessage))
      }
    }
  }

  const { loading : editOfficeLoading, error : editOfficeError } = useSelector(
    (state: RootState) => state.editOfficeReducer
  );

  const editOffice = async (editOfficeModel: EditOfficeModel) => {
    if (token) {
      try {
        dispatch(editOfficeRequest())
        await PUT(editOfficeModel, token)
        dispatch(editOfficeSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editOfficeFailure(errorMessage))
      }
    }
  }

  const { loading : deleteOfficeLoading, error : deleteOfficeError } = useSelector(
    (state: RootState) => state.deleteOfficeReducer
  );

  const deleteOffice = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteOfficeRequest())
        await DELETE(id, token)
        dispatch(deleteOfficeSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deleteOfficeFailure(errorMessage))
      }
    }
  }

  return {
    fetchOfficesLoading, fetchOfficesError, fetchOfficesData,
    createOfficeLoading, createOfficeError,
    editOfficeLoading, editOfficeError,
    deleteOfficeLoading, deleteOfficeError,
    fetchOffices, editOffice, createOffice, deleteOffice, fetchOfficesWithPagination }
}