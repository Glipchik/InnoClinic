import { useDispatch, useSelector } from "react-redux"
import { DELETE, GETWithPagination, POST, PUT, GET as serviceCategoryGET } from "../api/serviceCategoryApi"

import {
  fetchServiceCategoriesFailure,
  fetchServiceCategoriesSuccess,
  fetchServiceCategoriesRequest
} from "../../store/slices/serviceCategories/fetchServiceCategoriesSlice"

import {
  createServiceCategoryFailure,
  createServiceCategorySuccess,
  createServiceCategoryRequest
} from "../../store/slices/serviceCategories/createServiceCategorySlice"

import {
  editServiceCategoryFailure,
  editServiceCategorySuccess,
  editServiceCategoryRequest
} from "../../store/slices/serviceCategories/editServiceCategorySlice"

import {
  deleteServiceCategoryFailure,
  deleteServiceCategorySuccess,
  deleteServiceCategoryRequest
} from "../../store/slices/serviceCategories/deleteServiceCategorySlice"

import { RootState } from "../../store/store";
import ServiceCategory from "../../entities/serviceCategory";
import CreateServiceCategoryModel from "../../models/serviceCategories/createServiceCategoryModel";

export const useServiceCategories = (token: string | null) => {
  const dispatch = useDispatch()

  const { loading : fetchServiceCategoriesLoading, error : fetchServiceCategoriesError, data : fetchServiceCategoriesData } = useSelector(
    (state: RootState) => state.fetchServiceCategoriesReducer
  );

  const fetchServiceCategories = async () => {
    if (token) {
      try {
        dispatch(fetchServiceCategoriesRequest())
        const response = await serviceCategoryGET(null, token)
        dispatch(fetchServiceCategoriesSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(
          fetchServiceCategoriesFailure(errorMessage),
        )
      }
    }
  }

  const fetchServiceCategoriesWithPagination = async (pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchServiceCategoriesRequest())
        const response = await GETWithPagination(pageIndex, pageSize, token)
        dispatch(fetchServiceCategoriesSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(
          fetchServiceCategoriesFailure(errorMessage),
        )
      }
    }
  }

  const { loading : createServiceCategoryLoading, error : createServiceCategoryError } = useSelector(
    (state: RootState) => state.createServiceCategoriesReducer
  );

  const createServiceCategory = async (createServiceCategoryModel: CreateServiceCategoryModel) => {
    if (token) {
      try {
        dispatch(createServiceCategoryRequest())
        await POST(createServiceCategoryModel, token)
        dispatch(createServiceCategorySuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createServiceCategoryFailure(errorMessage))
      }
    }
  }

  const { loading : editServiceCategoryLoading, error : editServiceCategoryError } = useSelector(
    (state: RootState) => state.editServiceCategoriesReducer
  );

  const editServiceCategory = async (serviceCategory: ServiceCategory) => {
    if (token) {
      try {
        dispatch(editServiceCategoryRequest())
        await PUT(serviceCategory, token)
        dispatch(editServiceCategorySuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
        dispatch(editServiceCategoryFailure(errorMessage))
      }
    }
  }

  const { loading : deleteServiceCategoryLoading, error : deleteServiceCategoryError } = useSelector(
    (state: RootState) => state.deleteServiceCategoriesReducer
  );

  const deleteServiceCategory = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteServiceCategoryRequest())
        await DELETE(id, token)
        dispatch(deleteServiceCategorySuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
        dispatch(deleteServiceCategoryFailure(errorMessage))
      }
    }
  }

  return {
    fetchServiceCategoriesLoading, fetchServiceCategoriesError, fetchServiceCategoriesData,
    createServiceCategoryLoading, createServiceCategoryError,
    editServiceCategoryLoading, editServiceCategoryError,
    deleteServiceCategoryLoading, deleteServiceCategoryError,
    fetchServiceCategories, editServiceCategory, createServiceCategory, fetchServiceCategoriesWithPagination, deleteServiceCategory }
}