import { useDispatch, useSelector } from "react-redux"
import { DELETE, GET as doctorGET, GETWithPagination, POST, PUT } from "../api/doctorApi"
import { fetchDoctorsFailure, fetchDoctorsSuccess, fetchDoctorsRequest } from "../../store/slices/doctors/fetchDoctorsSlice"
import { RootState } from "../../store/store";
import CreateDoctorModel from "../../models/doctors/CreateDoctorModel";
import { createDoctorFailure, createDoctorRequest, createDoctorSuccess } from "../../store/slices/doctors/createDoctorSlice";
import Doctor from "../../entities/doctor";
import { editDoctorFailure, editDoctorRequest, editDoctorSuccess } from "../../store/slices/doctors/editDoctorSlice";
import { deleteDoctorFailure, deleteDoctorRequest, deleteDoctorSuccess } from "../../store/slices/doctors/deleteDoctorSlice";
import EditDoctorModel from "../../models/doctors/EditDoctorModel";

export const useDoctors = (token: string | null) => {
  const dispatch = useDispatch()
  const { loading : fetchDoctorsLoading, error : fetchDoctorsError, data : fetchDoctorsData } = useSelector(
    (state: RootState) => state.fetchDoctorsReducer
  );

  const fetchDoctors = async (specializationId: string) => {
    if (token) {
      try {
        dispatch(fetchDoctorsRequest())
        const response = await doctorGET(null, specializationId, token)
        dispatch(fetchDoctorsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(fetchDoctorsFailure(errorMessage))
      }
    }
  }

  const fetchDoctorsWithPagination = async (specializationId: string | null, officeId: string | null, pageIndex: number | null, pageSize: number | null) => {
    if (token) {
      try {
        dispatch(fetchDoctorsRequest())
        const response = await GETWithPagination(specializationId, officeId, pageIndex, pageSize, token)
        dispatch(fetchDoctorsSuccess(response.data))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }

        dispatch(fetchDoctorsFailure(errorMessage))
      }
    }
  }
  
  const { loading : createDoctorLoading, error : createDoctorError } = useSelector(
    (state: RootState) => state.createDoctorReducer
  );

  const createDoctor = async (createDoctorModel: CreateDoctorModel) => {
    if (token) {
      try {
        dispatch(createDoctorRequest())
        await POST(createDoctorModel, token)
        dispatch(createDoctorSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(createDoctorFailure(errorMessage))
      }
    }
  }

  const { loading : editDoctorLoading, error : editDoctorError } = useSelector(
    (state: RootState) => state.editDoctorReducer
  );

  const editDoctor = async (editDoctorModel: EditDoctorModel) => {
    if (token) {
      try {
        dispatch(editDoctorRequest())
        await PUT(editDoctorModel, token)
        dispatch(editDoctorSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(editDoctorFailure(errorMessage))
      }
    }
  }

  const { loading : deleteDoctorLoading, error : deleteDoctorError } = useSelector(
    (state: RootState) => state.deleteDoctorReducer
  );

  const deleteDoctor = async (id: string) => {
    if (token) {
      try {
        dispatch(deleteDoctorRequest())
        await DELETE(id, token)
        dispatch(deleteDoctorSuccess())
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";

        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
          
        dispatch(deleteDoctorFailure(errorMessage))
      }
    }
  }

  return {
    fetchDoctorsLoading, fetchDoctorsError, fetchDoctorsData,
    createDoctorLoading, createDoctorError,
    editDoctorLoading, editDoctorError,
    deleteDoctorLoading, deleteDoctorError,
    fetchDoctors, editDoctor, createDoctor, deleteDoctor, fetchDoctorsWithPagination }
}