import { useDispatch, useSelector } from "react-redux"
import { GET as officeGET, PUT, POST } from "../api/officeApi"
import { fetchOfficesDataFailure, fetchOfficesDataSuccess, fetchOfficesDataRequest } from "../../store/slices/officesSlice"
import { RootState } from "../../store/store";
import Office from "../../entities/office";
import CreateOfficeModel from "../../features/offices/models/CreateOfficeModel";

export const useOffices = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading, error, officesData } = useSelector(
    (state: RootState) => state.offices
  );

  const fetchOffices = async (pageIndex: number | null, padeSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchOfficesDataRequest())
        const response = await officeGET(pageIndex, padeSize, token)
        dispatch(fetchOfficesDataSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchOfficesDataFailure(errorMessage))
      }
    }
  }

  const createOffice = async (createOfficeModel: CreateOfficeModel) => {
    if (token) {
      try {
        dispatch(fetchOfficesDataRequest())
        await POST(createOfficeModel, token)
        dispatch(fetchOfficesDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchOfficesDataFailure(errorMessage))
      }
    }
  }

  const editOffice = async (office: Office) => {
    if (token) {
      try {
        dispatch(fetchOfficesDataRequest())
        await PUT(office, token)
        dispatch(fetchOfficesDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchOfficesDataFailure(errorMessage))
      }
    }
  }

  return { loading, error, officesData, fetchOffices, editOffice, createOffice }
}