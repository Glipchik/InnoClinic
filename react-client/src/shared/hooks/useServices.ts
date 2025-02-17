import { useDispatch, useSelector } from "react-redux"
import { GET, POST, GETWithPagination, DELETE, PUT } from "../api/serviceApi"
import {
  fetchServicesFailure,
  fetchServicesSuccess,
  fetchServicesRequest,
} from "../../store/slices/services/fetchServicesSlice"

import {
  createServiceFailure,
  createServiceRequest,
  createServiceSuccess,
} from "../../store/slices/services/createServiceSlice"

import {
  editServiceFailure,
  editServiceRequest,
  editServiceSuccess,
} from "../../store/slices/services/editServiceSlice"

import {
  deleteServiceSuccess,
  deleteServiceRequest,
  deleteServiceFailure,
} from "../../store/slices/services/deleteServiceSlice"

import { RootState } from "../../store/store";
import CreateServiceModel from "../../models/services/CreateServiceModel";
import EditServiceModel from "../../models/services/EditServiceModel"

export const useServices = (token: string | null) => {
  const dispatch = useDispatch()

  const { loading : fetchServicesLoading, error : fetchServicesError, data : fetchServicesData } = useSelector(
    (state: RootState) => state.fetchServicesReducer
  );
  
  const fetchServices = async (specializationId: string | null) => {
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
  
  const fetchServicesWithPagination = async (pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchServicesRequest())
        const response = await GETWithPagination(pageIndex, pageSize, token)
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

  const { loading : createServiceLoading, error : createServiceError } = useSelector(
    (state: RootState) => state.createServiceReducer
  );

  const createService = async (createServiceModel: CreateServiceModel) => {
    if (token) {
      try {
        dispatch(createServiceRequest())
        await POST(createServiceModel, token)
        dispatch(createServiceSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createServiceFailure(errorMessage))
      }
    }
  }

  const { loading : editServiceLoading, error : editServiceError } = useSelector(
    (state: RootState) => state.editServiceReducer
  );

  const editService = async (editServiceModel: EditServiceModel) => {
    if (token) {
      try {
        dispatch(editServiceRequest())
        await PUT(editServiceModel, token)
        dispatch(editServiceSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editServiceFailure(errorMessage))
      }
    }
  }

  const { loading : deleteServiceLoading, error : deleteServiceError } = useSelector(
    (state: RootState) => state.deleteServiceReducer
  );

  const deleteService = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteServiceRequest())
        await DELETE(id, token)
        dispatch(deleteServiceSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deleteServiceFailure(errorMessage))
      }
    }
  }

  return {
    fetchServicesLoading, fetchServicesError, fetchServicesData,
    createServiceLoading, createServiceError,
    editServiceError, editServiceLoading,
    deleteServiceLoading, deleteServiceError,
    createService,
    deleteService,
    editService,
    fetchServices,
    fetchServicesWithPagination
  }
}